using UnityEngine;
using Cysharp.Threading.Tasks;

public class Actor : MonoBehaviour
{
    private UniTaskCompletionSource turnEndTcs;

    private readonly ActorEventBus eventBus = new();
    private readonly ActorStatus status = new();

    public ActorEventBus EventBus => eventBus;
    public ActorStatus Status => status; 

    readonly private ActorEventPayload actorEventPayload = new();
    
    public void BeginTurn()
    {
        turnEndTcs = new UniTaskCompletionSource();

        actorEventPayload.Init();
        actorEventPayload.source = this;
        actorEventPayload.eventId = ActorEvent.TurnStart;

        eventBus.Dispatch(actorEventPayload);
    }

    public UniTask WaitForTurnEndAsync()
    {
        return turnEndTcs?.Task ?? UniTask.CompletedTask;
    }

    public void EndTurn()
    {
        actorEventPayload.Init();
        actorEventPayload.source = this;
        actorEventPayload.eventId = ActorEvent.TurnEnd;
        
        eventBus.Dispatch(actorEventPayload);
        turnEndTcs?.TrySetResult();
    }
}
