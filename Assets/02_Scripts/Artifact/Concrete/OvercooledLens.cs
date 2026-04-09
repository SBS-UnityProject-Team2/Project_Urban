using UnityEngine;

public class OvercooledLens : Artifact
{
	public override ArtifactId Id => ArtifactId.OvercooledLens;
	public override string KoreanName => "과냉각 렌즈";

	public override void Init(Actor actor)
	{
		bool isApplyingBonus = false;

		// Attack 이벤트는 기본 공격 계산/적용이 끝난 뒤 발생
		// 따라서 여기서 보내는 추가 피해는 "기본 타격 직후" 타이밍
		actor.EventBus.AddEventListener(ActorEvent.Attack, payload =>
		{
			if (isApplyingBonus)
				return;

			Actor target = payload.target;
			if (target == null)
				return;

			StatusEffect frost = target.Status.EffectList.GetEffect(StatusEffectName.Frost);
			if (frost == null || frost.Stack == 0)		// 대상에게 Frost가 없으면 추가타 발동X
				return;

			// Frost 1스택당 고정 피해 3.
			int additionalDamage = frost.Stack * 3;
			Debug.Log($"과냉각 렌즈 유물 추가데미지 {additionalDamage} 발생");

			// 기본 공격 직후 이벤트에서 추가타를 적용
			ActionPayload damagePayload = new()
			{
				actionId = ActorAction.AtkFixedDmg,
				source = actor,
			};

			// AtkFixedDmg 규칙: [Target, ElementType, Damage]
			damagePayload.AddTarget(target);
			damagePayload.Write(ElementType.None);		
			damagePayload.Write(additionalDamage);

			isApplyingBonus = true;
			ActionBus.Dispatch(damagePayload);
			isApplyingBonus = false;
		});
	}
}
