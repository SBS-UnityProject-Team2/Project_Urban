using UnityEngine;
public abstract class Buff : Card
{
    [SerializeField] protected int turns;
    public override CardType Type => CardType.Buff;
}