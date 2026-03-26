using UnityEngine;

public class CardInstance
{
    static int id = 0;

    private readonly int instanceId;
    private readonly CardName name;
    private bool isEnchanted = false;
    
    public CardName Name => name;
    public int InstanceId => instanceId;

    // 나중에 캐시처리하기
    public CardDataEntry CardData => CardManager.Instance.GetCardData(name);
    
    public CardInstance(CardName cardName)
    {
        name = cardName;
        instanceId = id++;
    }

    public void Enchant()
    {
        isEnchanted = true;
    }

    public T Instantiate<T>(T cardPrefab, Vector3 position, Transform parent) where T : Object, ICardInstance
    {
        T cardView = Object.Instantiate(cardPrefab, position, Quaternion.identity, parent);
        cardView.Init(this);

        return cardView;
    }
}