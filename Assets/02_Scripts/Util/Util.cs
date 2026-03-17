using UnityEngine;
using Cysharp.Threading.Tasks;

static public class Util
{
    static public async UniTask<ParticleSystem> PlayEffect
    (
        ParticleSystem effectPrefab, 
        Transform target, 
        Vector3 offset, 
        float duration, 
        bool autoDestroy = true
        )
    {
        ParticleSystem effect = Object.Instantiate(effectPrefab,offset, Quaternion.identity,target);
        effect.Play();

        await UniTask.WaitForSeconds(duration);

        if (autoDestroy)
        {
            Object.Destroy(effect.gameObject);
            
            return null;
        }

        return effect;
    }
} 