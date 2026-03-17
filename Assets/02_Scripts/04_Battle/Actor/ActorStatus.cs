using UnityEngine.Events;

public class ActorStatus
{
    private int curHp;
    private int maxHp;
    private int block;
    private int curCost;
    private int maxCost;
    private Element element;

    public UnityEvent<int, int> OnUpdateHp = new(); 
    public UnityEvent<int> OnUpdateBlock = new();
    public UnityEvent<int, int> OnUpdateCost = new();
    public UnityEvent<Element> OnUpdateElement = new();

    public int Hp 
    {
        get => curHp;
        set
        {
            curHp = value;

            if (curHp > maxHp)
                curHp = maxHp;

            if (curHp < 0)
                curHp = 0;

            OnUpdateHp?.Invoke(curHp, maxHp);
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

            OnUpdateHp?.Invoke(curHp, maxHp);
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

            OnUpdateBlock?.Invoke(block);
        }
    }

    public int Cost
    {
        get => curCost;
        set 
        {
            curCost = value;

            if (curCost < 0)
                curCost = 0;

            OnUpdateCost?.Invoke(curCost, maxCost);
        }
    }

    public Element Element 
    {   
        get => element; 
        set 
        {
            element = value;
            OnUpdateElement?.Invoke(element);
        }
    }

    public void Init(int curHp, int maxHp, int cost)
    {
        this.curHp = curHp;
        this.maxHp = maxHp;

        block = 0;

        curCost = cost;
        maxCost = cost;
    }
}