public class EntangledVines : Artifact
{
	private int discardCountInTurn;

	public override ArtifactId Id => ArtifactId.EntangledVines;
	public override string KoreanName => "얽혀진 덩굴";

	public override void Init(Actor actor)
	{
		// 턴 단위 누적 카운터 초기화
		discardCountInTurn = 0;

		// 1) TurnStart 이벤트 수신
		//    payload 내용 자체는 사용하지 않고, "새 턴 시작" 시점만 트리거로 사용
		actor.EventBus.AddEventListener(ActorEvent.TurnStart, _ =>
		{
			// 턴이 바뀌면 누적 버린 카드 수를 리셋
			discardCountInTurn = 0;
		});

		// 2) DiscardCard 이벤트 수신
		//    카드가 버려질 때마다 카운트를 올리고, 5번째에 회복 액션을 발행
		actor.EventBus.AddEventListener(ActorEvent.DiscardCard, _ =>
		{
			discardCountInTurn++;
			if (discardCountInTurn != 5)
				return;

			// 3) ActionBus로 전달할 ActionPayload 구성
			//    - actionId: HealHp
			//    - source  : 효과를 발생시킨 주체
			ActionPayload healPayload = new()
			{
				actionId = ActorAction.HealHp,
				source = actor,
			};

			// 4) HealHp 규칙에 맞는 대상/인자를 payload에 적재
			//    [Target, HealAmount]
			healPayload.AddTarget(actor);
			healPayload.Write(3);

			// 5) Dispatch 시 실제 HP 회복 수행
			ActionBus.Dispatch(healPayload);
		});
	}
}
