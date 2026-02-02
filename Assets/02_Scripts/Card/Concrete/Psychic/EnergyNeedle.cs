using System.Collections;
using UnityEngine;

public class EnergyNeedle : Attack
{
    [SerializeField] private int costGain = 1;

    public override CardName Name => CardName.EnergyNeedle;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        
        target.Damage(user, damage, Element.Psychic);
        
        int finalGain = costGain;
        if (user.Cost.CurrentCost - curCost == 0)
        {
            finalGain += 1;
        }
        
        user.Cost.Increase(finalGain);
    }
}