using UnityEngine.Events;

public class HealthController
{
    private int curHp;
    private int maxHp;
    private int protect;

    public int CurrentHp => curHp;
    public int MaxHp => maxHp;
    public int Protect => protect; 

    public UnityEvent<int,int, int> OnUpdate { get; } = new();

    public HealthController(int maxHp)
    {
        this.maxHp = maxHp;
        curHp = maxHp;
    }

    public void RefillHp()
    {
        curHp = maxHp;

        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void IncreaseHp(int amount)
    {
        curHp += amount;

        if (curHp > maxHp)
            curHp = maxHp;

        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void DecreaseHp(int amount)
    {
        curHp -= amount;

        if (curHp < 0)
            curHp = 0;

        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void IncreaseProtect(int amount)
    {
        protect += amount;
        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void DecreaseProtect(int amount)
    {
        protect -= amount;

        if (protect < 0)
            protect = 0;

        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void ExpandMaxHp(int amount)
    {
        maxHp += amount;
        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void ReduceMaxHp(int amount)
    {
        maxHp -= amount;

        if (maxHp < 0)
            maxHp = 0;

        if (maxHp < curHp)
            curHp = maxHp;

        OnUpdate?.Invoke(curHp, maxHp, protect);
    }

    public void ResetProtect()
    {
        protect = 0;
        OnUpdate?.Invoke(curHp, maxHp, protect);
    }
}