using UnityEngine;

public class AmmoNecklace : Artifact
{
	public override ArtifactId Id => ArtifactId.AmmoNecklace;
	public override string KoreanName => "탄약 목걸이";

	public override void Init(Actor actor)
	{
		actor.EventBus.AddEventListener(ActorEvent.ExtinctCard, payload =>
		{
			Monster monster = Battle.Instance.Monsters[Random.Range(0, Battle.Instance.Monsters.Count)];

			ActionPayload stPayload = new()
			{
				actionId = ActorAction.GiveBuffSta,
				source = actor,
			};
			stPayload.AddTarget(monster);
			stPayload.Write(StatusEffectName.Burn);
			stPayload.Write(1);

			ActionBus.Dispatch(stPayload);
		});
	}
}
