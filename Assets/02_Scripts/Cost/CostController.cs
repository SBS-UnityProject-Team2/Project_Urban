using UnityEngine.Events;

public class CostController
{
    private int curCost;
    private int maxCost;

    public CostController(int maxCost)
    {
        this.maxCost = maxCost;
        curCost = maxCost;
    }

    public int CurrentCost => curCost;
    public int MaxCost => maxCost;
    public UnityEvent<int, int> OnUpdateCost { get; } = new();

    public void IncreaseCost(int amount = 1)
    {
        curCost += amount;

        if (curCost > maxCost)
            curCost = maxCost;

        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void DecreaseCost(int amount = 1)
    {
        curCost -= amount;

        if (curCost < 0)
            curCost = 0;

        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void ExpandMaxCost(int amount)
    {
        maxCost += amount;
        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void ReduceMAxCost(int amount)
    {
        maxCost -= amount;

        if (maxCost < 0)
            maxCost = 0;

        if (maxCost < curCost)
            curCost = maxCost;

        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void ResetCost()
    {
        curCost = maxCost;
        OnUpdateCost?.Invoke(curCost, maxCost);
    }
}