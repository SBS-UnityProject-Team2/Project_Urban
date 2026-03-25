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

    bool isTurn = false;
    
    public void BeginTurn()
    {
        turnEndTcs = new UniTaskCompletionSource();

        actorEventPayload.Init();
        actorEventPayload.source = this;
        actorEventPayload.eventId = ActorEvent.TurnStart;

        eventBus.Dispatch(actorEventPayload);
        isTurn = true;
    }

    public UniTask WaitForTurnEndAsync()
    {
        return turnEndTcs?.Task ?? UniTask.CompletedTask;
    }

    public void EndTurn()
    {
        if (isTurn)
            EndTurnAsync().Forget();
    }

    private async UniTaskVoid EndTurnAsync()
    {
        actorEventPayload.Init();
        actorEventPayload.source = this;
        actorEventPayload.eventId = ActorEvent.TurnEnd;
        isTurn = false;
        
        await eventBus.DispatchAsync(actorEventPayload);
        turnEndTcs?.TrySetResult();
    }
}
