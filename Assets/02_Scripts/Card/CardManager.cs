using UnityEngine;
using System.Collections.Generic;

public class CardManager : Singleton<CardManager>
{
    [Header("Data")]
    [SerializeField] private CardData cardData; 

    public Card CreateCard(CardName cardName, Transform transform, bool isEnchanted)
    {
        CardDataEntry cardDataEntry = GetCardData(cardName);
        Card card = Instantiate(cardDataEntry.cardPrefab, transform);

        card.Init(cardDataEntry);

        if (isEnchanted) 
            //card.Enhance();       << 잠시 비활성화해놨음

        card.gameObject.SetActive(false);
        
        return card;
    }
    
    public CardDataEntry GetCardData(CardName name) => cardData.GetCardData(name);
    public CardDataEntry GetEnchantedCardData(CardName name) => cardData.GetEnchantedCardData(name);
    public List<CardDataEntry> GetCardsByElement(Element element) => cardData.GetCardsByElement(element);
    public List<CardName> GetAllCardNames() => cardData.GetAllCardNames();
    public List<CardDataEntry> GetAllCardData() => cardData.GetAllCardData();

    public CardName GetRandomCard(Element element)
    {
        List<CardDataEntry> dataEntries = GetCardsByElement(element);

        return dataEntries[Random.Range(0, dataEntries.Count)].cardName;
    }
}