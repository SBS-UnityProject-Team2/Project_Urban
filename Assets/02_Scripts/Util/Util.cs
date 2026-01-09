static class Util
{
    static public Element GetElement(CardName name)
    {
        int nameNum = (int)name;

        if (nameNum >= (int)Element.Bio)
            return Element.Bio;

        if (nameNum >= (int)Element.Psychic)
            return Element.Psychic;

        if (nameNum >= (int)Element.Ruin)
            return Element.Ruin;

        return Element.None;
    }
}