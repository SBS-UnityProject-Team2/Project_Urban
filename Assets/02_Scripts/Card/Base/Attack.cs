using UnityEngine;

abstract public class Attack : Card
{
    [SerializeField] protected int damage;
    public override CardType Type => CardType.Attack;
}