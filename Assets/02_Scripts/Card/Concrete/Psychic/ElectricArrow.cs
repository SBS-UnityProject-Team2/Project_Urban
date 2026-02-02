using System.Collections;
using UnityEngine;

public class ElectricArrow : Attack
{
    [SerializeField] private int drawCount = 1;

    public override CardName Name => CardName.ElectricArrow;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        
        target.Damage(user, damage, Element.Psychic);
        
        int finalDrawCount = drawCount;
        if (target.Status.Frozen.IsActive)
        {
            finalDrawCount++;
        }
        
        user.DrawCard(finalDrawCount);
    }
}