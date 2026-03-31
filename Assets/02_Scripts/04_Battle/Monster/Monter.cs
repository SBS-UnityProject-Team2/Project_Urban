public class Monster : Actor
{
    private void Awake()
    {
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}