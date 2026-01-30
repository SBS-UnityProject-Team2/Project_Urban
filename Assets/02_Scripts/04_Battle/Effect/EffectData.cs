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

    public GameObject GetEffectPrefab(EffectType effectType)
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
            JsonEffectWrapper wrapper = JsonUtility.FromJson<JsonEffectWrapper>(jsonText);

            effects.Clear();

            foreach (JsonEffectData jsonEffect in wrapper.effects)
            {
                if (!Enum.TryParse(jsonEffect.effectType, true, out EffectType parsedEffectType)) continue;

                EffectDataEntry entry = new()
                {
                    effectType = parsedEffectType,
                    effectName = jsonEffect.effectName ?? string.Empty,
                    effectPattern = ParseEffectPattern(jsonEffect.effectPattern),
                    effectDuration = ParseEffectDuration(jsonEffect.effectDuration),
                };

                // Prefab 로드
                string prefabPath = GetEffectPrefabPath(jsonEffect.effectPath);
                entry.effectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (entry.effectPrefab == null)
                {
                    Debug.LogWarning($"Effect prefab not found at: {prefabPath}");
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
        // 예: "1,3;2,4;3,9" → [[1,3], [2,4], [3,9]]
        if (string.IsNullOrEmpty(pattern)) return new int[0, 0];

        string[] rows = pattern.Split(';');
        if (rows.Length == 0) return new int[0, 0];

        string[] firstRowValues = rows[0].Split(',');
        int[,] result = new int[rows.Length, firstRowValues.Length];

        for (int i = 0; i < rows.Length; i++)
        {
            string[] values = rows[i].Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                if (int.TryParse(values[j].Trim(), out int value))
                {
                    result[i, j] = value;
                }
            }
        }

        return result;
    }

    private float[] ParseEffectDuration(string duration)
    {
        // 예: "0.2,0.3,0.6" → [0.2f, 0.3f, 0.6f]
        if (string.IsNullOrEmpty(duration)) return new float[0];

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
    public string effectType;        // 이펙트 번호
    public string effectName;        // 이펙트 이름
    public string effectPath;        // "Slash.prefab" or "Effects/Slash"
    public string effectPattern;     // "1,3;2,4;3,9" (세미콜론으로 행 구분, 쉼표로 열 구분)
    public string effectDuration;    // 각 단계별 대기 시간
}
#endif

[Serializable]
public class EffectDataEntry
{
    public EffectType effectType;
    public string effectName;
    public GameObject effectPrefab;
    public int[,] effectPattern;     // 이펙트 출력 위치 패턴 (route)
    public float[] effectDuration;   // 각 단계별 대기 시간
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