using UnityEngine;

public class DeckCard
{
    static int id = 0;

    private readonly int cardId;
    private readonly CardName name;
    private bool isEnchanted = false;
    
    public CardName Name => name;
    public int Id => cardId;

    // 나중에 캐시처리하기
    public CardDataEntry CardData => CardManager.Instance.GetCardData(name);
    
    public DeckCard(CardName cardName)
    {
        name = cardName;
        cardId = id++;
    }

    public void Enchant()
    {
        isEnchanted = true;
    }
}