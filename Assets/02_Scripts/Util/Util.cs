using System.Linq;

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

    static public int [] ParseIntArray(string intArrayString)
    {   
        if (intArrayString == string.Empty)
            return null;

        return intArrayString.Split(',').Select(numString => int.Parse(numString)).ToArray();
    }

    static public CardName [] ParseCardNameArray(string intArrayString)
    {
        if (intArrayString == string.Empty)
            return null;

        return intArrayString.Split(',').Select(numString => (CardName)int.Parse(numString)).ToArray();
    }
}