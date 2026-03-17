using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "StatusEffect/StatusEffectData", order = 0)]
public class StatusEffectData : ScriptableObject
{
    [SerializeField] private string spriteIconPath = "03_Images/Icon";
    [SerializeField] private List<StatusEffectDataEntry> statusEffects = new();

    private readonly Dictionary<StatusEffectName, StatusEffectDataEntry> effectMap = new();

    private void OnEnable()
    {
        foreach (StatusEffectDataEntry data in statusEffects)
            effectMap[data.effectName] = data;
    }
    public StatusEffectDataEntry GetEffectData(StatusEffectName effectName)
    {
        return effectMap[effectName];
    }

#if UNITY_EDITOR
    [ContextMenu("Import From Json")]
    public void ImportFromJson()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Standard Card Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonStatusEffectWrapper wrapper = JsonUtility.FromJson<JsonStatusEffectWrapper>(jsonText);
            
            statusEffects.Clear();

            foreach (JsonStatusEffectData jsonEffect in wrapper.statusEffects)
            {
                if (!Enum.TryParse(jsonEffect.effectName, true, out StatusEffectName parsedName))
                {
                    Debug.Log($"{jsonEffect.effectName} fail parse to Enum");
                    continue;
                }    

                string iconPath = GetAssetPath(spriteIconPath, jsonEffect.buffIcon, ".png");

                StatusEffectDataEntry dataEntry = new()
                {
                    effectName = parsedName,
                    koreanName = jsonEffect.koreanName,
                    description = jsonEffect.description,
                    buffIcon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath)  
                };

                ColorUtility.TryParseHtmlString(jsonEffect.color, out dataEntry.color);
                statusEffects.Add(dataEntry);
            }            
        }

        catch
        {
            
        }
    }

    private string GetAssetPath(string basePath, string fileName, string extension)
    {
        string relativePath = Path.Combine(basePath, fileName + extension);

        return Path.Combine("Assets", relativePath).Replace("\\", "/");
    }
#endif
}

[Serializable]
public class JsonStatusEffectWrapper
{
    public List<JsonStatusEffectData> statusEffects;
}

[Serializable]
public class JsonStatusEffectData
{
    public string effectName;
    public string koreanName;
    public string description;
    public string buffIcon;
    public string color;
}

[Serializable]
public class StatusEffectDataEntry
{
    public StatusEffectName effectName;
    public string koreanName;
    public string description;
    public Sprite buffIcon;
    public Color color;
}