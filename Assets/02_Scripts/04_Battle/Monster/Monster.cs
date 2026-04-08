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

    private void Awake()
    {
        action = GetComponent<MonsterAction>();
        view = GetComponent<MonsterView>();
        controller = GetComponent<MonsterController>();

        EventBus.AddAsyncEventListener(ActorEvent.TurnStart, HandleTurnStart);
        EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public void Init(MonsterDataEntry monsterDataEntry)
    {
        Status.Init(this, monsterDataEntry.hp, monsterDataEntry.hp, 0, ElementType.None);

        view.Init(monsterDataEntry, Status, action);
        action.Init(this, monsterDataEntry);
        controller.Init(view);
    }

    private async UniTask HandleTurnStart(EventPayload eventPayload)
    {
        await action.Execute(this);
        EndTurn();
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    { 
        if (Status.Health.CurHp <= action.NextPhaseHp)
            action.SetNextPhase();
        else
            action.SetNextAction();

        Status.Health.Block = 0;
    }

    private void HandleTakeDamage(EventPayload eventPayload)
    {
        if (Status.Health.CurHp <= action.NextPhaseHp)
            action.SetNextPhase();
    }
}