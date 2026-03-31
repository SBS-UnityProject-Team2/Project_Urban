public class ActorStatus
{
    private Health health;
    private Cost cost;
    private Element element;
    private StatusEffectList effectList;

    public Health Health => health;
    public Cost Cost => cost;
    public Element Element => element;
    public StatusEffectList EffectList => effectList;


    public void Init(Actor owner, int curHp, int maxHp, int initCost, ElementType initType)
    {
        health = new(curHp, maxHp);
        cost = new(initCost);
        element = new(initType);
        effectList = new(owner);
    }
}