using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "EffectData", menuName = "Effect/EffectData", order = 0)]
public class EffectData : ScriptableObject
{
    [SerializeField] private string effectPrefabBasePath = "05_Prefabs/Effects";
    private static readonly int[,] EmptyPattern = new int[0, 0];
    private static readonly float[] EmptyDuration = Array.Empty<float>();
    
    [Header("Data Lists")]
    [SerializeField] private List<EffectDataEntry> effects = new();

    private Dictionary<EffectType, EffectDataEntry> effectTypeMap;

    private void OnEnable()
    {
        BuildEffectDataMap();
    }

    private void BuildEffectDataMap()
    {
        effectTypeMap ??= new Dictionary<EffectType, EffectDataEntry>(effects.Count);
        effectTypeMap.Clear();

        foreach (EffectDataEntry entry in effects)
        {
            if (entry.effectPatternSerialized != null)
                entry.effectPattern = entry.effectPatternSerialized.ToArray();

            effectTypeMap[entry.effectType] = entry;
        }
    }

    public EffectControl GetEffectPrefab(EffectType effectType) => effectTypeMap[effectType].effectPrefab;

    public EffectDataEntry GetEffectData(EffectType effectType) => effectTypeMap[effectType];

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

                int[,] parsedPattern = ParseEffectPattern(jsonEffect.effectPattern);
                
                EffectDataEntry entry = new()
                {
                    effectType = parsedEffectType,
                    effectName = jsonEffect.effectName ?? string.Empty,
                    effectPattern = parsedPattern,
                    effectPatternSerialized = SerializablePattern.FromArray(parsedPattern),
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
        if (string.IsNullOrEmpty(pattern))
            return EmptyPattern;

        if (pattern.Contains("["))
            return ParseBracketedPattern(pattern);
        else
        {
            string[] values = pattern.Split(',');
            int[,] result = new int[1, values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (int.TryParse(values[i].Trim(), out int value))
                    result[0, i] = value;
            }
            return result;
        }
    }

    private int[,] ParseBracketedPattern(string pattern)
    {
        string[] rows = pattern.Split(new[] { "]," }, System.StringSplitOptions.None);
        if (rows.Length == 0) return EmptyPattern;

        List<int[]> parsedRows = new();
        for (int i = 0; i < rows.Length; i++)
        {
            string row = rows[i].Replace("[", "").Replace("]", "").Trim();
            if (string.IsNullOrEmpty(row))
                continue;
            
            string[] values = row.Split(',');
            int[] rowValues = new int[values.Length];
            for (int j = 0; j < values.Length; j++)
            {
                if (int.TryParse(values[j].Trim(), out int value))
                    rowValues[j] = value;
            }
            parsedRows.Add(rowValues);
        }

        if (parsedRows.Count == 0)
            return EmptyPattern;

        int maxCols = parsedRows[0].Length;
        int[,] result = new int[parsedRows.Count, maxCols];
        for (int i = 0; i < parsedRows.Count; i++)
        {
            for (int j = 0; j < parsedRows[i].Length; j++)
                result[i, j] = parsedRows[i][j];
        }
        
        return result;
    }

    private float[] ParseEffectDuration(string duration)
    {
        if (string.IsNullOrEmpty(duration)) return EmptyDuration;

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
            return EmptyDuration;
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
    public string effectPattern;     // "[3], [4], [9]" (프리펩 이동경로) 또는 "[4, 9]" 또는 "5" (단일)
    public string effectDuration;    
}
#endif

[Serializable]
public class SerializablePattern
{
    public int[] data;     
    public int rowCount;
    public int colCount;
    
    public int[,] ToArray()
    {
        if (data == null || data.Length == 0 || rowCount == 0 || colCount == 0)
            return new int[0, 0];
        
        int[,] result = new int[rowCount, colCount];
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
                result[i, j] = data[i * colCount + j];
        }
        
        return result;
    }
    
    public static SerializablePattern FromArray(int[,] array)
    {
        SerializablePattern pattern = new();
        if (array == null)
            return pattern;
        
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        
        pattern.rowCount = rows;
        pattern.colCount = cols;
        pattern.data = new int[rows * cols];
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                pattern.data[i * cols + j] = array[i, j];
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
    
    [System.NonSerialized]
    public int[,] effectPattern;

    public SerializablePattern effectPatternSerialized;
    public float[] effectDuration;

#if UNITY_EDITOR
    public string PatternDisplay
    {
        get
        {
            if (effectPatternSerialized == null || effectPatternSerialized.data == null)
                return "Empty";
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < effectPatternSerialized.rowCount; i++)
            {
                sb.Append("[");
                for (int j = 0; j < effectPatternSerialized.colCount; j++)
                {
                    sb.Append(effectPatternSerialized.data[i * effectPatternSerialized.colCount + j]);
                    if (j < effectPatternSerialized.colCount - 1)
                        sb.Append(", ");
                }
                sb.Append("]");
                if (i < effectPatternSerialized.rowCount - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }
    }
#endif
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