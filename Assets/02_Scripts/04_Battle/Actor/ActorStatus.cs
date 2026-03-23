using UnityEngine.Events;

public class ActorStatus
{
    private Health health;
    private Cost cost;
    private Element element;

    public Health Health => health;
    public Cost Cost => cost;

    public UnityEvent<Element> OnUpdateElement = new();

    public Element Element 
    {   
        get => element; 
        set 
        {
            element = value;
            OnUpdateElement?.Invoke(element);
        }
    }

    public void Init(int curHp, int maxHp, int initCost, Element initElement)
    {
        health = new(curHp, maxHp);
        cost = new(initCost);
        element = initElement;
    }
}