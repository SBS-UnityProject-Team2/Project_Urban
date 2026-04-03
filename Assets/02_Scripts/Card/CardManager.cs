using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [Header("Data")]
    [SerializeField] private CardData cardData; 
    [SerializeField] private EnchantCardData enchantCardData;

    [SerializeField] private ActionData actionData;
    
    // EffectData 도 ActionData 처럼 연결하기

    public Card GetCardPrefab()
    {
        return cardData.Prefab;
    }

    public CardDataEntry GetCardData(CardName cardName)
    {
        return cardData[cardName];
    }

    public EnchantCardDataEntry GetEnchantCardData(CardName cardName)
    {
        return enchantCardData[cardName];
    }

    public List<CardName> GetAllCardNames()
    {
        return cardData.CardNames;
    }

    public Sprite GetCardImage(CardName cardName)
    {
        return cardData.GetCardImage(cardName);
    }
    

    public List<ActionDataEntry> GetActionData(int linkId)
    {
        return actionData[linkId];
    }

    public CardName GetRandomCard(ElementType element)
    {
        List<CardName> allNames = GetAllCardNames();

        List<CardName> filtered = element == ElementType.None
            ? allNames
            : allNames.Where(name => Util.GetElement(name) == element).ToList();

        return filtered[Random.Range(0, filtered.Count)];
    }

    public string GetDescription(CardName cardName)
    {
        CardDataEntry cardDataEntry = GetCardData(cardName);
        string description = cardDataEntry.description ?? string.Empty;

        // 상점/덱 UI 설명은 CardData에 저장된 값으로 치환 (ActionData 의존 제거)
        description = description.Replace("[ActValue1]", cardDataEntry.actValue1.ToString());
        description = description.Replace("[ActValue2]", cardDataEntry.actValue2.ToString());
        description = description.Replace("[ActValue3]", cardDataEntry.actValue3.ToString());

        return description;
    }
}