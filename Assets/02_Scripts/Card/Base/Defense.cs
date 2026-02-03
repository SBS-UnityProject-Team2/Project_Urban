using UnityEngine;
public abstract class Defense : Card
{
    [SerializeField] protected int protect;

    public override CardType Type => CardType.Defense;
}