using UnityEngine.Events;

public class CostController
{
    private int curCost;
    private int maxCost;
    private int recoveryCost;

    public CostController(int maxCost)
    {
        this.maxCost = maxCost;
        curCost = maxCost;
        recoveryCost = maxCost;
    }

    public int CurrentCost => curCost;
    public int MaxCost => maxCost;
    public UnityEvent<int, int> OnUpdateCost { get; } = new();

    public void Increase(int amount = 1)
    {
        curCost += amount;
        
        if (curCost > maxCost)
            curCost = maxCost;

        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void Decrease(int amount = 1)
    {
        curCost -= amount;

        if (curCost < 0)
            curCost = 0;

        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void ExpandMax(int amount)
    {
        int diff = maxCost - recoveryCost;

        maxCost += amount;
        recoveryCost = maxCost - diff;
        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void ReduceMax(int amount)
    {
        int diff = maxCost - recoveryCost;
        maxCost -= amount;

        if (maxCost < 0)
            maxCost = 0;

        if (maxCost < curCost)
            curCost = maxCost;

        recoveryCost = maxCost - diff;

        if (recoveryCost < 0)
            recoveryCost = 0;

        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void Reset()
    {
        curCost = maxCost;
        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void Recovery()
    {
        curCost = recoveryCost;
        OnUpdateCost?.Invoke(curCost, maxCost);
    }

    public void IncreaseRecovery(int amount = 1)
    {
        recoveryCost += amount;
    }

    public void DecreaseRecovery(int amount = 1)
    {
        recoveryCost -= amount;

        if (recoveryCost < 0)
            recoveryCost = 0;
    }
}