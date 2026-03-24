using UnityEngine;
using Cysharp.Threading.Tasks;

static public class Util
{
    static public async UniTask MoveTo(GameObject gameObject, Vector3 destination, float duration)
    {
        float curTime = 0.0f;
        Vector3 startPos = gameObject.transform.position;

        while (curTime < duration)
        {
            float t = curTime / duration;
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);

            gameObject.transform.position = Vector3.Lerp(startPos, destination, smoothT);
            curTime += Time.deltaTime;

            await UniTask.Yield();
        }
    }
} 