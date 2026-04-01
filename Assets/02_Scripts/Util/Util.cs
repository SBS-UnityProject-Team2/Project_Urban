using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

static public class Util
{
    static public async UniTask MoveTo(GameObject gameObject, Vector3 destination, float duration)
    {
        float curTime = 0.0f;
        Vector3 startPos = gameObject.transform.localPosition;

        while (curTime < duration)
        {
            float t = curTime / duration;
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);

            gameObject.transform.localPosition = Vector3.Lerp(startPos, destination, smoothT);
            curTime += Time.deltaTime;

            await UniTask.Yield();
        }
    }

    static public ElementType GetElement(CardName name)
    {
        int nameNum = (int)name;

        if (nameNum >= (int)ElementType.Bio)
            return ElementType.Bio;

        if (nameNum >= (int)ElementType.Psychic)
            return ElementType.Psychic;

        if (nameNum >= (int)ElementType.Ruin)
            return ElementType.Ruin;

        return ElementType.None;
    }
    
    static public int [] ParseIntArray(string intArrayString)
    {   
        if (string.IsNullOrWhiteSpace(intArrayString))
            return null;

        return intArrayString.Split(',').Select(numString => int.Parse(numString.Trim())).ToArray();
    }

    static public CardName [] ParseCardNameArray(string intArrayString)
    {
        if (string.IsNullOrWhiteSpace(intArrayString))
            return null;

        return intArrayString.Split(',').Select(numString => (CardName)int.Parse(numString.Trim())).ToArray();
    }
        
} 