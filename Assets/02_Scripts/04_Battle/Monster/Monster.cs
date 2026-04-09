using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MonsterAction))]
[RequireComponent(typeof(MonsterView))]
[RequireComponent(typeof(MonsterController))]
public class Monster : Actor
{
    private MonsterAction action;
    private MonsterView view;
    private MonsterController controller;
    
    private int score;

    public MonsterView View => view;

    private void Awake()
    {
        action = GetComponent<MonsterAction>();
        view = GetComponent<MonsterView>();
        controller = GetComponent<MonsterController>();

        EventBus.AddAsyncEventListener(ActorEvent.TurnStart, HandleTurnStart);
        EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        EventBus.AddEventListener(ActorEvent.DamageTaken, HandleTakeDamage);
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    public void Init(MonsterDataEntry monsterDataEntry)
    {
        Status.Init(this, monsterDataEntry.hp, monsterDataEntry.hp, 0, ElementType.None);

        view.Init(monsterDataEntry, Status, action);
        action.Init(this, monsterDataEntry);
        controller.Init(view);

        score = monsterDataEntry.score;
    }

    private async UniTask HandleTurnStart(EventPayload eventPayload)
    {
        Status.Health.Block = 0;

        var actionBlocks = Status.EffectList.GetActiveEffectWith<IActionBlock>();

        if (!actionBlocks.Any(block => block.IsActionBlocked()))
            await action.Execute(this);
            
        EndTurn();
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    { 
        if (Status.Health.CurHp <= action.NextPhaseHp)
            action.SetNextPhase();
        else
            action.SetNextAction();
    }

    private void HandleTakeDamage(EventPayload eventPayload)
    {
        if (Status.Health.CurHp <= action.NextPhaseHp)
            action.SetNextPhase();
    }

    private void HandleDead(EventPayload payload)
    {
        float monsterRatio = score * 0.1f + 1.0f;
        float randomRatio = Random.Range(-0.2f, 0.2f) + 1.0f;

        Battle.Instance.EarnCoin += (int)(10 * monsterRatio * randomRatio);
    }
}