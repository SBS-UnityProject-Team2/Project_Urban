using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

/*
    모듈 구분하기
    1. 카드 조작 로직
    2. 카드 동작 로직
    3. 카드 UI 로직
    4. 카드 이펙트 로직
*/

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(UICard))]
public class Card : MonoBehaviour, ICardView
{
    // UI 모듈
    private UICard uICard;

    private int instanceId;
    private CardDataEntry cardData;
    private List<ActionDataEntry> actionData;
    private readonly ActionPayload payload = new();
    
    public CardDataEntry CardData => cardData;

    private void Awake()
    {
        uICard = GetComponent<UICard>();
    }

    public void SetCardDataEntry(int instanceId, CardDataEntry data)
    {
        this.instanceId = instanceId;
        cardData = data;
        uICard.SetCardDataEntry(data);
        actionData = CardManager.Instance.GetActionData(cardData.linkId);
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
                case Target.TarSelf:
                    payload.targets.Add(payload.source);
                    break;

                case Target.TarEnemy:
                    payload.targets.Add(selectedActor);
                    break;

                case Target.TarEnemyRandom:
                case Target.TarEnemyRandomEach:
                    {
                        int count = Battle.Instance.Monsters.Count;
                        payload.targets.Add(Battle.Instance.Monsters[Random.Range(0, count)]);
                        break;
                    }

                case Target.TarEnemiesAll:
                    payload.targets.AddRange(Battle.Instance.Monsters);
                    break;


                // 적 위치 관련된 부분은 몬스터 관리자 작성 후 추가하기
                case Target.TarAdjacentEnemies:
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