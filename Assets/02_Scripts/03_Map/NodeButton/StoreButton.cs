public class StoreButton : NodeButton
{
    public override void OnClick()
    {
        ButtonEvent.Instance.OnClickStore();
    }
}