public class NormalButton : NodeButton
{
    public override void OnClick()
    {
        ButtonEvent.Instance.OnClickNormal();
    }
}