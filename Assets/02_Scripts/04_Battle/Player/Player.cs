using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(PlayerView))]
public class Player : Actor
{
    [SerializeField] private PlayerView playerView;

    public ElementType LastUsedElementType { get; set; }
    public List<Artifact> artifacts = new();

    private void Awake()
    {
        playerView = GetComponent<PlayerView>();

        Status.Init(this, PlayerManager.Instance.Health.CurrentHp, PlayerManager.Instance.Health.MaxHp, 10, ElementType.None);
        foreach(ArtifactId artifactId in PlayerManager.Instance.Artifacts.List)
        {
            Artifact artifact = ArtifactFactory.Create(artifactId);
            artifact.Init(this);
            artifacts.Add(artifact);
        }
        
        playerView.Init(Status);

        EventBus.AddAsyncEventListener(ActorEvent.TurnStart, HandleTurnStart);
        EventBus.AddAsyncEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    private async UniTask HandleTurnStart(EventPayload eventPayload)
    {
        RegenCost();
        await InitDraw();

        // 조작 가능한 상태로 변경
    }

    private async UniTask HandleTurnEnd(EventPayload eventPayload)
    {
        Status.Health.Block = 0;

        await Battle.Instance.Deck.DiscardAllCard();

        // 조작 불가능한 상태로 변경
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }

    private async UniTask InitDraw()
    {
        int drawCount = Battle.Instance.DrawCount;
        List<IDrawCountChange> drawCountChanges = Status.EffectList.GetActiveEffectWith<IDrawCountChange>();
        drawCountChanges.ForEach(draw => drawCount += draw.GetDrawCountDelta());

        await Battle.Instance.Deck.DrawCard(drawCount);

        InitDrawPayload payload = new()
        {
            source = this,
            target = this,
            drawCount = drawCount
        };

        DispatchEvent(payload);
    }

    private void RegenCost()
    {
        int regenCost = Status.Cost.MaxCost;
        List<ICostRegenChange> costRegenChanges = Status.EffectList.GetActiveEffectWith<ICostRegenChange>();
        costRegenChanges.ForEach(cost => regenCost += cost.GetCostDelta());

        Status.Cost.CurCost = regenCost;
    }   
}