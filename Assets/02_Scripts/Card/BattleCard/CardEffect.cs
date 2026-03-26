using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    private Transform target;
    private List<ParticleSystem> particles;
    private List<List<Vector3>> offsets;
    private List<float> durations;

    public void Init(int effectType)
    {
        // effectType으로 데이터 찾아서 보관하기       
    }

    public async UniTask Play()
    {
        List<UniTask> tasks = new();
        List<ParticleSystem> effects = new();

        int idx = 0;
        foreach (ParticleSystem particlePrefab in particles)
        {
            ParticleSystem effect = EffectHelper.CreateEffect(particlePrefab, target);
            effects.Add(effect);

            UniTask task = EffectHelper.PlayEffect(effect, offsets[0][idx++], durations[0]);
            tasks.Add(task);
        }

        await UniTask.WhenAll(tasks);
        tasks.Clear();

        Vector3 originPost = target.transform.position;
        for (int i = 1; i < offsets.Count; i++)
        {
            for (int j = 0; j < offsets[i].Count; j++)
            {
                UniTask task = EffectHelper.MoveEffect(effects[j], originPost + offsets[i][j], durations[i]);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
            tasks.Clear();
        }

        foreach (ParticleSystem effect in effects)
            Destroy(effect.gameObject);
    }
}