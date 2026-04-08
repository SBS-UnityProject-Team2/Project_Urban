using UnityEngine.Events;

public class EntangledVines : Artifact
{
	private UnityAction<EventPayload> onTurnStart;
	private UnityAction<EventPayload> onDiscardCard;
	private int discardCountInTurn;

	public override ArtifactId Id => ArtifactId.EntangledVines;
	public override string KoreanName => "얽혀진 덩굴";

	public override void Init(Actor actor)
	{
		discardCountInTurn = 0;

		onTurnStart = _ => { discardCountInTurn = 0; };
		onDiscardCard = _ =>
		{
			discardCountInTurn++;
			if (discardCountInTurn != 5)
				return;

			ActionPayload healPayload = new()
			{
				actionId = ActorAction.HealHp,
				source = actor,
			};
			healPayload.AddTarget(actor);
			healPayload.Write(3);

			ActionBus.Dispatch(healPayload);
		};

		actor.EventBus.AddEventListener(ActorEvent.TurnStart, onTurnStart);
		actor.EventBus.AddEventListener(ActorEvent.DiscardCard, onDiscardCard);
	}

	public override void Dispose(Actor actor)
	{
		actor.EventBus.RemoveEventListener(ActorEvent.TurnStart, onTurnStart);
		actor.EventBus.RemoveEventListener(ActorEvent.DiscardCard, onDiscardCard);

		onTurnStart = null;
		onDiscardCard = null;
		discardCountInTurn = 0;
	}
}
