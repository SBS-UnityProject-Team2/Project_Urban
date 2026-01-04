using System.Collections.Generic;

public class StatusEffectList
{
    private readonly List<TimedStatusEffect> effectList = new();
    private readonly Target owner;

    public StatusEffectList(Target owner)
    {
        this.owner = owner;
    }

    public void AddEffects(TimedStatusEffect statusEffect)
    {
        statusEffect.Apply(owner);
        effectList.Add(statusEffect);
    }

    public void DecreaseTurn()
    {
        for (int i = effectList.Count - 1; i >= 0; i--)
        {
            TimedStatusEffect effect = effectList[i];
            effect.DecreaseTurn();

            if (effect.RemainingTurn <= 0)
            {
                effectList.RemoveAt(i);
                effect.Revert(owner);
            }
        }
    }
}