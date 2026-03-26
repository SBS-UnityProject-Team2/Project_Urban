using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData", order = 0)]
public class CardData : ScriptableObject
{
    [Header("Prefab Setting")]
    [SerializeField] private Card cardPrefab;

    [Header("Card Image Setting")]
    [SerializeField] private List<Sprite> images = new();

    [Header("Card Data List")]
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
    [ContextMenu("Import Card Data")]
    public void ImportCardData()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Standard Card Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) 
            throw new ArgumentException($"Can no open {jsonPath}");

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonCardWrapper jsonWrapper = JsonUtility.FromJson<JsonCardWrapper>(jsonText);

            entries.Clear();

            foreach (JsonCardData jsonCard in jsonWrapper.cards)
            {
                if (!Enum.TryParse(jsonCard.cardName, true, out CardName parsedCardName))
                    throw new ArgumentException($"{jsonCard.cardName} can not parse CardName");

                if (!Enum.TryParse(jsonCard.element, true, out Element parsedElement))
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
                    effectType = jsonCard.effectType,
                    linkId = jsonCard.linkId
                };

                entries.Add(cardDataEntry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            BuildCardDataMap();
            Debug.Log($"Successfully imported {entries.Count} cards.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import CardData JSON: {e.Message}");
        }
    }
#endif
}

#if UNITY_EDITOR
[Serializable]
public class JsonCardWrapper
{
    public List<JsonCardData> cards;
}

[Serializable]
public class JsonCardData
{
    public string cardName;
    public string koreanName;
    public string element;
    public string description;
    public bool isExtinct;
    public bool isSpecial;
    public int cost;
    public int price;
    public int effectType;
    public int linkId;
}
#endif

[Serializable]
public class CardDataEntry
{
    public CardName cardName;
    public string koreanName;
    public Element element;
    public string description;
    public bool isExtinct;
    public bool isSpecial;
    public int cost;
    public int price;

    // 나중에 이펙트 타입으로 변경하기
    public int effectType;

    public int linkId;
}
