public class ShelterButton : NodeButton
{
    public override void OnClick()
    {
        ButtonEvent.Instance.OnClickShelter();
    }
}