using UnityEngine;

abstract public class BuffCard : Card
{
    [SerializeField] protected int turns;

    public override CardType Type => CardType.BuffCard;
}