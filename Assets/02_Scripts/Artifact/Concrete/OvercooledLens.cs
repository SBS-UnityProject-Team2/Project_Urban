using UnityEngine.Events;

public class OvercooledLens : Artifact
{
	private UnityAction<EventPayload> onAttack;

	public override ArtifactId Id => ArtifactId.OvercooledLens;
	public override string KoreanName => "과냉각 렌즈";

	public override void Init(Actor actor)
	{
		onAttack = payload =>
		{
			Actor target = payload.target;
			if (target == null)
				return;

			StatusEffect frost = target.Status.EffectList.GetEffect(StatusEffectName.Frost);
			if (frost == null || frost.Stack <= 0)
				return;

			int additionalDamage = frost.Stack * 3;

			ActionPayload damagePayload = new()
			{
				actionId = ActorAction.AtkFixedDmg,
				source = actor,
			};
			damagePayload.AddTarget(target);
			damagePayload.Write(ElementType.None);
			damagePayload.Write(additionalDamage);

			ActionBus.Dispatch(damagePayload);
		};

		actor.EventBus.AddEventListener(ActorEvent.Attack, onAttack);
	}

	public override void Dispose(Actor actor)
	{
		actor.EventBus.RemoveEventListener(ActorEvent.Attack, onAttack);
		onAttack = null;
	}
}
