using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "EffectData", menuName = "Effect/EffectData", order = 0)]
public class EffectData : ScriptableObject
{
    [SerializeField] private string effectPrefabBasePath = "05_Prefabs/Effects";
    
    [Header("Data Lists")]
    [SerializeField] private List<EffectDataEntry> effects = new();

    // EffectType별로 직접 조회
    private Dictionary<EffectType, EffectDataEntry> effectTypeMap;

    private void OnEnable()
    {
        BuildEffectDataMap();
    }

    private void BuildEffectDataMap()
    {
        effectTypeMap = new Dictionary<EffectType, EffectDataEntry>();

        foreach (EffectDataEntry entry in effects)
        {
            effectTypeMap[entry.effectType] = entry;
        }
    }

    public EffectControl GetEffectPrefab(EffectType effectType)
    {
        return effectTypeMap[effectType].effectPrefab;
    }

    public EffectDataEntry GetEffectData(EffectType effectType)
    {
        return effectTypeMap[effectType];
    }

    public List<EffectDataEntry> GetAllEffectData()
    {
        return new List<EffectDataEntry>(effects);
    }

    public IEnumerable<EffectDataEntry> GetAllEffects()
    {
        return effects;
    }

#if UNITY_EDITOR

    [ContextMenu("Import Effects From JSON")]
    public void ImportEffectsFromJson()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Effect Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            
            // JSON이 배열인 경우 처리
            List<JsonEffectData> effectsList = new();
            if (jsonText.TrimStart().StartsWith("["))
            {
                // 배열을 객체 래퍼로 감싸기
                jsonText = "{\"effects\":" + jsonText + "}";
            }
            
            JsonEffectWrapper wrapper = JsonUtility.FromJson<JsonEffectWrapper>(jsonText);

            effects.Clear();

            foreach (JsonEffectData jsonEffect in wrapper.effects)
            {
                // effectType이 int이므로 직접 캐스팅
                if (!System.Enum.IsDefined(typeof(EffectType), jsonEffect.effectType))
                {
                    Debug.LogWarning($"Invalid effect type: {jsonEffect.effectType}");
                    continue;
                }
                
                EffectType parsedEffectType = (EffectType)jsonEffect.effectType;

                EffectDataEntry entry = new()
                {
                    effectType = parsedEffectType,
                    effectName = jsonEffect.effectName ?? string.Empty,
                    effectPattern = SerializablePattern.FromArray(ParseEffectPattern(jsonEffect.effectPattern)),
                    effectDuration = ParseEffectDuration(jsonEffect.effectDuration),
                };

                // Prefab 로드
                string prefabPath = GetEffectPrefabPath(jsonEffect.effectPath);
                GameObject prefabGO = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                
                if (prefabGO != null)
                {
                    entry.effectPrefab = prefabGO.GetComponent<EffectControl>();
                }
                else
                {
                    Debug.LogWarning($"Prefab not found at path: {prefabPath}");
                }

                effects.Add(entry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            BuildEffectDataMap();

            Debug.Log($"Successfully imported {effects.Count} effects.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import Effect JSON: {e.Message}");
        }
    }

    private int[,] ParseEffectPattern(string pattern)
    {
        if (string.IsNullOrEmpty(pattern)) return new int[0, 0];

        // "[0], [2]" 또는 "[4, 9]" 형식: 대괄호로 감싼 형식
        if (pattern.Contains("["))
        {
            return ParseBracketedPattern(pattern);
        }
        // "5" 또는 "1,2,3" 형식: 직접 숫자
        else
        {
            string[] values = pattern.Split(',');
            int[,] result = new int[1, values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (int.TryParse(values[i].Trim(), out int value))
                {
                    result[0, i] = value;
                }
            }
            return result;
        }
    }

    private int[,] ParseBracketedPattern(string pattern)
    {
        // "[0], [2]" → [[0], [2]]
        // "[4, 9]" → [[4, 9]]
        // "[3], [4], [9]" → [[3], [4], [9]]
        
        string[] rows = pattern.Split(new[] { "]," }, System.StringSplitOptions.None);
        if (rows.Length == 0) return new int[0, 0];

        // 각 행에서 값 추출
        List<int[]> parsedRows = new();
        for (int i = 0; i < rows.Length; i++)
        {
            string row = rows[i].Replace("[", "").Replace("]", "").Trim();
            string[] values = row.Split(',');
            int[] rowValues = new int[values.Length];
            for (int j = 0; j < values.Length; j++)
            {
                if (int.TryParse(values[j].Trim(), out int value))
                {
                    rowValues[j] = value;
                }
            }
            parsedRows.Add(rowValues);
        }

        if (parsedRows.Count == 0) return new int[0, 0];

        // 모든 행이 같은 길이라고 가정하고 2D 배열 생성
        int maxCols = parsedRows[0].Length;
        int[,] result = new int[parsedRows.Count, maxCols];
        for (int i = 0; i < parsedRows.Count; i++)
        {
            for (int j = 0; j < parsedRows[i].Length; j++)
            {
                result[i, j] = parsedRows[i][j];
            }
        }
        return result;
    }

    private float[] ParseEffectDuration(string duration)
    {
        if (string.IsNullOrEmpty(duration)) return new float[0];

        // 쉼표로 구분된 여러 값: "0.2, 0.3, 0.6" 또는 "0.4, 1"
        if (duration.Contains(","))
        {
            string[] values = duration.Split(',');
            float[] result = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (float.TryParse(values[i].Trim(), out float value))
                {
                    result[i] = value;
                }
            }
            return result;
        }
        // 단일 숫자: "1" 또는 "2.5"
        else
        {
            if (float.TryParse(duration.Trim(), out float value))
            {
                return new float[] { value };
            }
            return new float[0];
        }
    }

    private string GetEffectPrefabPath(string effectPath)
    {
        // effectPath가 이미 전체 경로면 그대로, 아니면 basePath 결합
        if (effectPath.StartsWith("Assets/"))
        {
            return effectPath;
        }
        
        string relativePath = Path.Combine(effectPrefabBasePath, effectPath);
        
        // .prefab 확장자가 없으면 추가
        if (!relativePath.EndsWith(".prefab"))
        {
            relativePath += ".prefab";
        }
        
        return Path.Combine("Assets", relativePath).Replace("\\", "/");
    }
#endif
}

#if UNITY_EDITOR
[Serializable]
public class JsonEffectWrapper
{
    public List<JsonEffectData> effects;
}

[Serializable]
public class JsonEffectData
{
    public int effectType;           // 이펙트 번호
    public string effectName;        // 이펙트 이름
    public string effectPath;        // "Slash.prefab" or "Effects/Slash"
    public string effectPattern;     // "[1], [2]" 또는 "[4, 9]" 또는 "5"
    public string effectDuration;    // "0.2, 0.3, 0.6" 또는 "1" 또는 "2.5"
}
#endif

[Serializable]
public class SerializablePattern
{
    public List<int[]> rows = new();
    
    public int[,] ToArray()
    {
        if (rows == null || rows.Count == 0) return new int[0, 0];
        int rowCount = rows.Count;
        int colCount = rows[0].Length;
        int[,] result = new int[rowCount, colCount];
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                result[i, j] = rows[i][j];
            }
        }
        return result;
    }
    
    public static SerializablePattern FromArray(int[,] array)
    {
        SerializablePattern pattern = new();
        if (array == null) return pattern;
        
        int rowCount = array.GetLength(0);
        int colCount = array.GetLength(1);
        
        for (int i = 0; i < rowCount; i++)
        {
            int[] row = new int[colCount];
            for (int j = 0; j < colCount; j++)
            {
                row[j] = array[i, j];
            }
            pattern.rows.Add(row);
        }
        return pattern;
    }
}

[Serializable]
public class EffectDataEntry
{
    public EffectType effectType;
    public string effectName;
    public EffectControl effectPrefab;
    public SerializablePattern effectPattern;
    public float[] effectDuration;
}

// EffectType enum 정의
public enum EffectType
{
    StormSlashHit = 0,
    StormSlash = 1,
    FireSlashHit = 2,
    FireFlame = 3,
    FireMeshGlow = 4,
    FirePillarBlast = 5,
    FireSphereBlast = 6,
    FireWall = 7,
    FrostSlashHit = 8,
    FrostSphereBlast = 9,
    AuraRing = 10,
    AuraCircling = 11,
    LifeSlashHit = 12,
    EarthBeamImpact = 13,
    AreaDamageEarth = 14,
    LifeSpray = 15,
    LifeBeamImpact = 16,
    EarthSphereBlast = 17,
    CurseLife = 18,
    FireFlame2 = 19,
    FrostWallCircle = 20,
    InfernoFireMeshGlow = 21,
    InfernoFirePillarBlast = 22
}