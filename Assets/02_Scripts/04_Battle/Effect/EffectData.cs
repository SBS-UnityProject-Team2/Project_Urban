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
            effectTypeMap[entry.effectType] = entry;
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

                EffectDataEntry entry = new()
                {
                    effectType = parsedEffectType,
                    effectName = jsonEffect.effectName ?? string.Empty,
                    effectPattern = ParseEffectPattern(jsonEffect.effectPattern),
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

    private List<IntRow> ParseEffectPattern(string pattern)
    {
        List<IntRow> result = new();

        if (string.IsNullOrEmpty(pattern))
            return result;

        string[] rows = pattern.Split(new[] { "]," }, StringSplitOptions.None);
        foreach (string row in rows)
        {
            string cleaned = row.Replace("[", "").Replace("]", "").Trim();
            if (string.IsNullOrEmpty(cleaned)) continue;

            IntRow intRow = new();
            foreach (string token in cleaned.Split(','))
            {
                if (int.TryParse(token.Trim(), out int value))
                    intRow.values.Add(value);
            }
            result.Add(intRow);
        }

        return result;
    }

    private List<float> ParseEffectDuration(string duration)
    {
        List<float> result = new();

        if (string.IsNullOrEmpty(duration))
            return result;

        foreach (string token in duration.Split(','))
        {
            if (float.TryParse(token.Trim(), out float value))
                result.Add(value);
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
    public int effectType;           // 이펙트 번호
    public string effectName;        // 이펙트 이름
    public string effectPath;        // "Slash.prefab" or "Effects/Slash"
    public string effectPattern;     // "[3], [4], [9]" (프리펩 이동경로) 또는 "[4, 9]" 또는 "5" (단일)
    public string effectDuration;    
}
#endif

[Serializable]
public class IntRow
{
    public List<int> values = new();
}

[Serializable]
public class EffectDataEntry
{
    public EffectType effectType;
    public string effectName;
    public EffectControl effectPrefab;
    public List<IntRow> effectPattern;
    public List<float> effectDuration;
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
    FrostWall = 20,
    InfernoFireMeshGlow = 21,
    InfernoFirePillarBlast = 22
}