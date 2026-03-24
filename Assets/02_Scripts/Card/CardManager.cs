using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [Header("Data")]
    [SerializeField] private CardData cardData; 
    [SerializeField] private ActionData actionData;

    public Card CreateCard(CardName cardName, Transform transform, bool isEnchanted)
    {
        CardDataEntry cardDataEntry = cardData[cardName];
        Card card = Instantiate(cardData.Prefab, transform);

        card.gameObject.SetActive(false);
        
        return card;
    }    

    public CardDataEntry GetCardData(CardName cardName)
    {
        return cardData[cardName];
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
}