public class EliteButton : NodeButton
{
    public override void OnClick()
    {
        ButtonEvent.Instance.OnClickElite();
    }
}