using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
#endif

[CreateAssetMenu(fileName = "EnchantCardData", menuName = "Card/EnchantCardData", order = 1)]
public class EnchantCardData : ScriptableObject
{
    [Header("Prefab Setting")]
    [SerializeField] private Card cardPrefab;

    [Header("Card Image Setting")]
    [SerializeField] private List<Sprite> images = new();

    [Header("Enchant Card Data List")]
    [SerializeField] private List<CardDataEntry> entries = new();

    private readonly Dictionary<CardName, Sprite> cardImageMap = new();
    private readonly Dictionary<CardName, CardDataEntry> cardDataMap = new();

    public Card Prefab => cardPrefab;

    public List<CardName> CardNames => cardDataMap.Keys.ToList();

    public CardDataEntry this[CardName cardName]
    {
        get => cardDataMap[cardName];
    }

    public Sprite GetCardImage(CardName cardName)
    {
        return cardImageMap[cardName];
    }

    private void OnEnable()
    {
        BuildCardDataMap();
    }

    private void BuildCardDataMap()
    {
        cardImageMap.Clear();
        cardDataMap.Clear();

        foreach (Sprite cardImage in images)
        {
            Debug.Assert(Enum.TryParse(cardImage.name, out CardName cardName));
            cardImageMap[cardName] = cardImage;
        }

        foreach (CardDataEntry cardData in entries)
            cardDataMap[cardData.cardName] = cardData;
    }

#if UNITY_EDITOR
    [ContextMenu("Import Enchant Card Data")]
    public void ImportEnchantCardData()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Enchant Card Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath))
            throw new ArgumentException($"Can no open {jsonPath}");

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            // effectType가 숫자/문자열 혼합으로 들어오는 포맷을 문자열 기준으로 통일
            jsonText = Regex.Replace(jsonText, "\"effectType\"\\s*:\\s*(\\d+)", "\"effectType\": \"$1\"");
            JsonEnchantCardWrapper jsonWrapper = JsonUtility.FromJson<JsonEnchantCardWrapper>(jsonText);

            entries.Clear();

            foreach (JsonEnchantCardData jsonCard in jsonWrapper.cards)
            {
                if (!Enum.TryParse(jsonCard.cardName, true, out CardName parsedCardName))
                    throw new ArgumentException($"{jsonCard.cardName} can not parse CardName");

                if (!Enum.TryParse(jsonCard.element, true, out ElementType parsedElement))
                    throw new ArgumentException($"{jsonCard.element} can not parse Element");

                CardDataEntry cardDataEntry = new()
                {
                    cardName = parsedCardName,
                    koreanName = jsonCard.koreanName,
                    element = parsedElement,
                    isExtinct = jsonCard.isExtinct,
                    isSpecial = jsonCard.isSpecial,
                    description = jsonCard.description,
                    price = jsonCard.price,
                    cost = jsonCard.cost,
                    effectType = ParseEffectType(jsonCard.effectType),
                    linkId = jsonCard.EnhancedLinkID,
                    actValue1 = jsonCard.ActValue1,
                    actValue2 = jsonCard.ActValue2,
                    actValue3 = jsonCard.ActValue3
                };

                entries.Add(cardDataEntry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            BuildCardDataMap();
            Debug.Log($"Successfully imported {entries.Count} enchant cards.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import EnchantCardData JSON: {e.Message}");
        }
    }

    private static int ParseEffectType(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return 0;
        if (int.TryParse(raw, out int single)) return single;

        string firstToken = raw.Split(',')[0].Trim();
        return int.TryParse(firstToken, out int parsed) ? parsed : 0;
    }
#endif
}

#if UNITY_EDITOR
[Serializable]
public class JsonEnchantCardWrapper
{
    public List<JsonEnchantCardData> cards;
}

[Serializable]
public class JsonEnchantCardData
{
    public string cardName;
    public string koreanName;
    public string element;
    public string description;
    public bool isExtinct;
    public bool isSpecial;
    public int cost;
    public int price;
    public string effectType;
    public int EnhancedLinkID;
    public int ActValue1;
    public int ActValue2;
    public int ActValue3;
}
#endif
