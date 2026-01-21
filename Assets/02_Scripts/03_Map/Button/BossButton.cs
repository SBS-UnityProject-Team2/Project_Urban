public class BossButton : NodeButton
{
    public override void OnClick()
    {
        ButtonEvent.Instance.OnClickBoss();
    }
}