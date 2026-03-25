using UnityEditor.Animations;
using UnityEngine;

public class CardInstance
{
    static int id = 0;

    private readonly int instanceId;
    private readonly CardName name;
    private bool isEnchanted = false;
    
    public CardName Name => name;
    public int InstanceId => instanceId;
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

    public T Instantiate<T>(T cardPrefab, Vector3 position, Transform parent) where T : Object, ICardView
    {
        T cardView = Object.Instantiate(cardPrefab, position, Quaternion.identity, parent);
        cardView.SetCardDataEntry(instanceId, CardData);

        return cardView;
    }
}