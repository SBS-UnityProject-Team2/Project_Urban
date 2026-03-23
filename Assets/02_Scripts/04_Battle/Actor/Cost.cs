using UnityEngine.Events;

public class Cost
{
    private int curCost;
    private int maxCost;

    public UnityEvent<int, int> OnUpdate = new();

    public Cost(int initCost)
    {
        curCost = initCost;
        maxCost = initCost;
    }

    public int CurCost
    {
        get => curCost;
        set
        {
            curCost = value;

            if (curCost < 0)
                curCost = 0;

            OnUpdate?.Invoke(curCost, maxCost);
        }
    }

    public int MaxCost
    {
        get => maxCost;
        set
        {
            maxCost = value;

            if (maxCost < 0)
                maxCost = 0;

            OnUpdate?.Invoke(curCost, maxCost);
        }
    }
}