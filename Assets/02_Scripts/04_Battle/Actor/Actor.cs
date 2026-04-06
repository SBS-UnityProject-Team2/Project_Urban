using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Actor : MonoBehaviour
{
    private UniTaskCompletionSource turnEndTcs;
    private CancellationToken token;
    protected CancellationTokenSource tokenSource;

    private readonly ActorEventBus eventBus = new();
    private readonly ActorStatus status = new();

    public ActorEventBus EventBus => eventBus;
    public ActorStatus Status => status;

    readonly private EventPayload actorEventPayload = new();

    private bool isTurn = false;
    public bool IsTurn => isTurn;
    
    public async UniTask BeginTurn()
    {
        RefreshToken();

        actorEventPayload.Init();
        actorEventPayload.source = this;
        actorEventPayload.eventId = ActorEvent.TurnStart;

        isTurn = true;
        await eventBus.DispatchAsync(actorEventPayload);
    }

    private void RefreshToken()
    {
        turnEndTcs = new();
        tokenSource = new();

        token = tokenSource.Token;
        token.Register(() => turnEndTcs.TrySetCanceled(token));
    }

    public UniTask WaitForTurnEndAsync()
    {
        return turnEndTcs.Task;
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
        Debug.Log("End Turn Complete");
    }

    public void DispatchEvent(EventPayload eventPayload)
    {
        eventBus.Dispatch(eventPayload);
    }
}
