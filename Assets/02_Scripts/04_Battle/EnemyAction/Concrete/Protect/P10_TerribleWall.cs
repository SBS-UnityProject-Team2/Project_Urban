using UnityEngine;

[CreateAssetMenu(fileName = "P10_TerribleWall", menuName = "Enemy/Actions/Protect/P10_TerribleWall", order = 10)]
public class P10_TerribleWall : EnemyAction
{
    [SerializeField] private int protectPoint = 10;
    [SerializeField] private int elasticVeilPoint = 1;

    public override ActionType Type => ActionType.Protect | ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
        target.ElasticVeil(elasticVeilPoint);
    }
}