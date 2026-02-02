using System.Collections;
using UnityEngine;

public class ElectricField : BuffCard
{   
    [SerializeField] private int damage; 

    public override CardName Name => CardName.ElectricField;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        user.ElectricVeil(damage);
    }
}