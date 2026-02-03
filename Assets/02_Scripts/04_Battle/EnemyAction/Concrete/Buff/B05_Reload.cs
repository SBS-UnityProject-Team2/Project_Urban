using UnityEngine;

[CreateAssetMenu(fileName = "B05_Reload", menuName = "Enemy/Actions/Buff/B05_Reload", order = 5)]
public class B05_Reload : EnemyAction
{
    [SerializeField] private int loadedIncendiaryPoint = 2;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        enemy.LoadedIncendiary(loadedIncendiaryPoint);
    }
}