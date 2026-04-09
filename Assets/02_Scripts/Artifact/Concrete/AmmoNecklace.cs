using UnityEngine;

public class AmmoNecklace : Artifact
{
	public override ArtifactId Id => ArtifactId.AmmoNecklace;
	public override string KoreanName => "탄약 목걸이";

	public override void Init(Actor actor)
	{
		// 1) actor에서 ExtinctCard 이벤트를 수신

		//    "카드 소멸이 발생했다"는 트리거 자체만 사용한
		actor.EventBus.AddEventListener(ActorEvent.ExtinctCard, payload =>
		{
			// 2) 효과 대상 선택: 몬스터 목록 중 임의 1개를 선택
			Monster monster = Battle.Instance.Monsters[Random.Range(0, Battle.Instance.Monsters.Count)];

			// 3) ActionBus로 전달할 ActionPayload 구성
			//    - actionId: 어떤 시스템 액션을 실행할지
			//    - source  : 누가 이 액션을 발동했는지
			ActionPayload stPayload = new()
			{
				actionId = ActorAction.GiveBuffSta,
				source = actor,
			};

			// 4) 액션 대상 및 파라미터를 순서대로 payload에 적재
			//    GiveBuffSta 규칙: [Target, StatusEffectName, Stack]
			stPayload.AddTarget(monster);
			stPayload.Write(StatusEffectName.Burn);
			stPayload.Write(1);
			
			//    실제 Burn 부여 로직을 실행
			ActionBus.Dispatch(stPayload);
		});
	}
}
