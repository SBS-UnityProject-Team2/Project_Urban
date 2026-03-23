using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    private UniTaskCompletionSource turnEndTcs;

    private ActorEventBus eventBus = new();
    private ActorStatus status = new();


    public ActorEventBus EventBus => eventBus;
    public ActorStatus Status => status; 

    readonly private ActorEventPayload actorEventPayload = new();

    private void Awake()
    {
        actorEventPayload.source = this;
    }
    
    public void BeginTurn()
    {
        turnEndTcs = new UniTaskCompletionSource();

        actorEventPayload.eventId = ActorEvent.TurnStart;
        eventBus.Dispatch(actorEventPayload);
    }

    public UniTask WaitForTurnEndAsync()
    {
        return turnEndTcs?.Task ?? UniTask.CompletedTask;
    }

    public void EndTurn()
    {
        actorEventPayload.eventId = ActorEvent.TurnEnd;
        eventBus.Dispatch(actorEventPayload);

        turnEndTcs?.TrySetResult();
    }

    public void TakeDamage(int damage)
    {
        
    }
}
