using UnityEngine;
using Cysharp.Threading.Tasks;

public static class EffectHelper
{
    static public ParticleSystem CreateEffect(ParticleSystem effectPrefab, Transform parent = null)
    {
        ParticleSystem effect = Object.Instantiate(effectPrefab);
        effect.transform.localPosition = Vector3.zero;

        if (parent != null)
            effect.transform.parent = parent;

        return effect;
    }

    static async public UniTask PlayEffect(ParticleSystem effect, Vector3 offset, float duration)
    {
        effect.transform.position += offset;
        effect.Play();

        await UniTask.WaitForSeconds(duration);
    }

    static async public UniTask MoveEffect(ParticleSystem effect, Vector3 destination, float moveDuration)
    {
        float moveTime = 0.0f;
        Vector3 startPos = effect.transform.position;

        while (moveTime < moveDuration)
        {
            effect.transform.position = Vector3.Lerp(startPos, destination, moveTime / moveDuration);
            moveTime += Time.deltaTime;

            await UniTask.Yield();
        }

        effect.transform.position = destination;
    }
} 