using Cysharp.Threading.Tasks;

public class Monster : Actor
{
    private void Awake()
    {
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    private void HandleDead(ActorEventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}