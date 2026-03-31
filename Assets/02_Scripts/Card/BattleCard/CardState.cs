using UnityEngine;

public class CardState : MonoBehaviour
{
    [SerializeField] private int originCost;
    [SerializeField] private int curCost;

    public int Cost
    {
        get => curCost;
        set
        {
            curCost = value;

            if (value < 0)
                curCost = 0;
        }
    }

    public void Init(CardDataEntry cardDataEntry)
    {
        originCost = cardDataEntry.cost;
        curCost = originCost;
    }

    public void ResetCost()
    {
        curCost = originCost;
    }
}