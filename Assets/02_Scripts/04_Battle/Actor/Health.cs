using UnityEngine.Events;

public class Health
{
    private int curHp;
    private int maxHp;
    private int block;

    public UnityEvent<int, int, int> OnUpdate = new(); 

    public Health(int curHp, int maxHp)
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        block = 0;
    }

    public int CurHp 
    {
        get => curHp;
        set
        {
            curHp = value;

            if (curHp > maxHp)
                curHp = maxHp;

            if (curHp < 0)
                curHp = 0;

            OnUpdate?.Invoke(curHp, maxHp, block);
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value;

            if (maxHp < 0)
                maxHp = 0;

            OnUpdate?.Invoke(curHp, maxHp, block);
        }
    }

    public int Block 
    {
        get => block;
        set 
        {
            block = value;

            if (block < 0)
                block = 0;

            OnUpdate?.Invoke(curHp, maxHp, block);
        }
    }
}