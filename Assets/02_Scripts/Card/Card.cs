using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    private CardDataEntry cardData;
    private List<ActionDataEntry> actionData;
    private readonly ActionPayload payload = new();

    public CardDataEntry CardData => cardData;

    public void Init(CardName cardName)
    {
        cardData = CardManager.Instance.GetCardData(cardName);
        actionData = CardManager.Instance.GetActionData(cardData.linkId);
    }

    public Target GetTarget()
    {
        return actionData[0].actTarget;
    }

    public async void Use(Actor selectedActor)
    {
        foreach (ActionDataEntry action in actionData)
        {
            payload.Init();
            payload.actionId = action.actId;
            payload.source = Battle.Instance.Player;

            // 타겟 지정 로직
            switch (action.actTarget)
            {
                case Target.Self:
                    payload.targets.Add(payload.source);
                    break;

                case Target.Enemy:
                    payload.targets.Add(selectedActor);
                    break;

                case Target.EnemyRandom:
                case Target.EnemyRandomEach:
                {
                    int count = Battle.Instance.Monsters.Count;
                    payload.targets.Add(Battle.Instance.Monsters[Random.Range(0, count)]);
                    break;
                }

                case Target.EnemiesAll :
                    payload.targets.AddRange(Battle.Instance.Monsters);
                    break;
                

                // 적 위치 관련된 부분은 몬스터 관리자 작성 후 추가하기
                case Target.AdjacentEnemies :
                {
                    payload.targets.Add(selectedActor);


                    break;   
                }
            }

            payload.Write(action.actValue);
            // actParam처리하기

            ActionBus.Dispatch(payload);
            // 여기서 이펙트 작동시키기
        }
    }

    private async UniTask PlayEffect
    (
        Transform target,
        List<ParticleSystem> particles,
        List<List<Vector3>> offsets,
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