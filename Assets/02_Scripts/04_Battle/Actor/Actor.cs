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

    public void BeginTurn()
    {
        RefreshToken();

        actorEventPayload.Init();
        actorEventPayload.source = this;
        actorEventPayload.eventId = ActorEvent.TurnStart;

        eventBus.Dispatch(actorEventPayload);
        isTurn = true;
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
    }

    public void DispatchEvent(EventPayload eventPayload)
    {
        eventBus.Dispatch(eventPayload);
    }
}
