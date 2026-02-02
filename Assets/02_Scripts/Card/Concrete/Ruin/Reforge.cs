using System.Collections;
using UnityEngine;

public class Reforge : Defense 
{
    [SerializeField] private int turn;

    public override CardName Name => CardName.Reforge;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int currentBurn = user.Status.Burn.Count;
        int totalShield = armor + currentBurn;
        
        yield return PlayEffect(target);
        target.Armor(totalShield);
        target.Refined(turn);
    }
}