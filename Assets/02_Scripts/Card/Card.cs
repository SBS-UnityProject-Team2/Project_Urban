using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    private ActorActionPayload payload = new();
    private CardDataEntry cardData;

    public CardDataEntry CardData => cardData;
    public CardName CardName => cardData != null ? cardData.cardName : default;

    public void Init(CardDataEntry data)
    {
        cardData = data;
    }

    public void Use()
    {
        // 등록된 카드 정보를 토대로 액션 및 이펙트 재생

        // 액션 정보 확인
        // payload에 값채우고 ActionBus로 보내서 액션 동작시키기
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