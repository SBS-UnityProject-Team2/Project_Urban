public class EventButton : NodeButton
{
    public override void OnClick()
    {
        ButtonEvent.Instance.OnClickEvent();
    }
}