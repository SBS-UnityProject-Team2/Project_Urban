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
        string descriptionTemplate = cardDataEntry.description ?? string.Empty;

        List<object> args = GetDescriptionArgs(cardDataEntry.linkId);
        if (args.Count == 0)
            return descriptionTemplate;

        return string.Format(descriptionTemplate, args.ToArray());
    }

    private List<object> GetDescriptionArgs(int linkId)
    {
        return actionData[linkId].Select(action => (object)action.actValue).ToList();
    }
}