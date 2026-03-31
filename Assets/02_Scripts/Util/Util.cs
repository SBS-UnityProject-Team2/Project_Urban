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