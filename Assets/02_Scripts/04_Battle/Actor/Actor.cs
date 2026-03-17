using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    private UniTaskCompletionSource turnEndTcs;

    public ActorEventBus eventBus = new();
    public ActorActionBus actionBus = new();
    public ActorStatus status = new();

    readonly private ActorEventPayload actorEventPayload = new();

    private void Awake()
    {
        actionBus.Bind(status, eventBus);
        actorEventPayload.source = this;
    }
    
    public void BeginTurn()
    {
        turnEndTcs = new UniTaskCompletionSource();

        actorEventPayload.eventId = ActorEvent.TurnStart;
        eventBus.Invoke(actorEventPayload);
    }

    public UniTask WaitForTurnEndAsync()
    {
        return turnEndTcs?.Task ?? UniTask.CompletedTask;
    }

    public void EndTurn()
    {
        actorEventPayload.eventId = ActorEvent.TurnEnd;
        eventBus.Invoke(actorEventPayload);

        turnEndTcs?.TrySetResult();
    }
}
