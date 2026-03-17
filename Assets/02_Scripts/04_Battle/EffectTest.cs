using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Vector3List
{
    public List<Vector3> values = new();
}

public class EffectTest : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private List<Vector3List> offsets;
    [SerializeField] private List<float> durations;

    private async void Awake()
    {   
        await PlayEffect(target, particles, offsets, durations);
    }

    private async UniTask PlayEffect
    (
        Transform target,
        List<ParticleSystem> particles, 
        List<Vector3List> offsets, 
        List<float> durations
    )
    {
        List<UniTask> tasks = new();
        List<ParticleSystem> effects = new();
        
        int idx = 0;
        foreach (ParticleSystem particlePrefab in particles)
        {
            ParticleSystem effect = EffectHelper.CreateEffect(particlePrefab, target);
            effects.Add(effect);

            UniTask task = EffectHelper.PlayEffect(effect, offsets[0].values[idx++], durations[0]);
            tasks.Add(task);
        }

        await UniTask.WhenAll(tasks);
        tasks.Clear();
        
        Vector3 originPost = target.transform.position;
        for (int i = 1; i < offsets.Count; i++)
        {
            for (int j = 0; j < offsets[i].values.Count; j++)
            {
                UniTask task = EffectHelper.MoveEffect(effects[j], originPost + offsets[i].values[j], durations[i]);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
            tasks.Clear();
        }

        foreach (ParticleSystem effect in effects)
            Destroy(effect.gameObject);
    }
}
