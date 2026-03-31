/*
    TurnEnd 이벤트가 이미 정의되어 있음.
    따라서 TurnEffect를 상속받는 Effect가 TurnEnd 시점에 어떤 동작을 해야한다면
    InternalHandleTurnEnd를 override해서 동작을 정의하면 됨.
*/
abstract public class DurationEffect : StatusEffect
{
    override public int StatusNumber => duration;

    public DurationEffect(Actor owner) : base(owner) {}
}