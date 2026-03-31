public class Summoned : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Summoned;

    public Summoned(Actor owner) : base(owner)
    {
    }

    // 몬스터 관리자 클래스 필요
    // 관리자 클래스에서 몬스터 사망 시, 다른 몬스터에게도 사망 이벤트를 전달
    // 다른 몬스터한테 Summoned 디버프가 부여되어 있는지 확인
    // 남은 몬스터에게 Summoned가 있다면 사망처리한다.
}