using UnityEngine.Events;

public class Element
{
    private ElementType originType;
    private ElementType curType;

    public ElementType Type => curType;
    public UnityEvent<ElementType> OnUpdate = new();

    public Element(ElementType initType)
    {
        originType = initType;
        curType = initType;
    }

    public void ChangeType(ElementType newType)
    {
        curType = newType;

        OnUpdate?.Invoke(curType);
    }

    public void Reset()
    {
        curType = originType;

        OnUpdate?.Invoke(curType);
    }
}