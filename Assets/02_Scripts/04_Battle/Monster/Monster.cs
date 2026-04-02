using Cysharp.Threading.Tasks;

[UnityEngine.RequireComponent(typeof(MonsterAction))]
[UnityEngine.RequireComponent(typeof(MonsterView))]
public class Monster : Actor
{
    private MonsterAction action;
    private MonsterView view;

    private void Awake()
    {
        action = GetComponent<MonsterAction>();
        view = GetComponent<MonsterView>();

        EventBus.AddAsyncEventListener(ActorEvent.TurnStart, HandleTurnStart);
        EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    public void Init(MonsterDataEntry monsterDataEntry)
    {
        action.Init(monsterDataEntry);
        view.Init(Status, action);
    }

    private async UniTask HandleTurnStart(EventPayload eventPayload)
    {
        await action.Execute();

        EndTurn();
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        // 방어도 0으로 만들기
        // 다음 액션 지정하기   
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
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}