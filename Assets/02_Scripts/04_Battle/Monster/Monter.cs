public class Monster : Actor
{
    private void Awake()
    {
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    private void HandleTurnStart(EventPayload eventPayload)
    {
        // 바로 지정된 액션을 실행한다.
        // 액션이 끝날때까지 대기
        // 액션이 끝나면 TurnEnd 호출
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        // 방어도 0으로 만들기
        // 다음 액션 지정하기   
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}