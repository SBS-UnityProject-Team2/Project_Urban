using UnityEngine;
using System.Collections.Generic;

public class CardManager : Singleton<CardManager>
{
    [Header("Data")]
    [SerializeField] private CardData cardData; 

    [Header("Card Spawn / Despawn Point")]
    [SerializeField] private Transform cardSpawnPoint;          // 카드가 생성될 위치
    [SerializeField] private Transform cardDespawnPoint;        // 카드가 소멸할 위치

    public Card CreateCard(CardName cardName, bool isEnchanted)
    {
        CardDataEntry cardDataEntry = GetCardData(cardName);
        Card card = Instantiate(cardDataEntry.cardPrefab, cardSpawnPoint.position, Quaternion.identity);

        card.Init(cardDataEntry);

        if (isEnchanted) 
            card.Enhance();
        
        return card;
    }
    
    public CardDataEntry GetCardData(CardName name) => cardData.GetCardData(name);
    public CardDataEntry GetEnchantedCardData(CardName name) => cardData.GetEnchantedCardData(name);
    public List<CardDataEntry> GetCardsByElement(Element element) => cardData.GetCardsByElement(element);

    public List<CardName> GetAllCardNames() => cardData.GetAllCardNames();
    public List<CardDataEntry> GetAllCardData() => cardData.GetAllCardData();
}