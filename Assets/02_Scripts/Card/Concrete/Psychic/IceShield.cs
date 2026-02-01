using System.Collections;
using UnityEngine;

public class IceShield : Defense
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.IceShield;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(armor);
        target.KineticVeil(turn);
    }
}