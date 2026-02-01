using System.Collections;
using UnityEngine;

public class MoltenArms : Attack 
{
    [SerializeField] private int burnCount = 5;

    public override CardName Name => CardName.MoltenArms;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {   
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Ruin);
        user.Burn(burnCount);
    }
}