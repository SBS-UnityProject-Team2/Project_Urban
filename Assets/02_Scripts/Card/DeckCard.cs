using System;

[Serializable]
public class DeckCard
{
    static int id = 0;

    private readonly int cardId;
    public CardName name;
    private bool isEnchanted = false;
    
    public CardName Name => name;
    public int Id => cardId;
    public bool IsEnchanted => isEnchanted;

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