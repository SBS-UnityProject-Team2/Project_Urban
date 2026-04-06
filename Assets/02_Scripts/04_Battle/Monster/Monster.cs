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
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    public void Init(MonsterDataEntry monsterDataEntry)
    {
        Status.Init(this, monsterDataEntry.hp, monsterDataEntry.hp, 0, ElementType.None);

        action.Init(monsterDataEntry);
        view.Init(monsterDataEntry, Status, action);
        controller.Init(view);
    }

    private async UniTask HandleTurnStart(EventPayload eventPayload)
    {
        await action.Execute();

        Debug.Log("Monster Action Complete!");
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

        // Damage받는 이펙트등을 재생
        Debug.Log("Take Damage");
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}