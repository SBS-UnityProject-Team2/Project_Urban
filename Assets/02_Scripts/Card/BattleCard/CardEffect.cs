using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    static readonly List<Vector3> dirs = new()
    {
        new Vector3(0, 5, 0),
        Vector3.up + Vector3.left,
        Vector3.up,
        Vector3.up + Vector3.right,
        Vector3.left,
        Vector3.zero,
        Vector3.right,
        Vector3.down + Vector3.left,
        Vector3.down,
        Vector3.down + Vector3.right,
        new Vector3(0, 4, 0),
        new Vector3(0, 1, 0)
    };

    static readonly float offset = 1.0f;
    private List<EffectDataEntry> effectDataList = new();

    public void Init(int[] effectType)
    {
        for (int i = 0; i < effectType.Length; i++)
        {
            EffectDataEntry dataEntry = EffectManager.Instance.GetEffectData((EffectType)effectType[i]);
            effectDataList.Add(dataEntry);
        }
    }

    public async UniTask Play(Actor target)
    {
        List<UniTask> tasks = new();

        foreach (EffectDataEntry effectData in effectDataList)
        {
            UniTask task = PlayEffect(effectData, target);
            tasks.Add(task);
        }

        await UniTask.WhenAll(tasks);
    }

    private async UniTask PlayEffect(EffectDataEntry effectDataEntry, Actor target)
    {
        List<IntRow> patterns = effectDataEntry.effectPattern;
        List<float> durations = effectDataEntry.effectDuration;

        EffectControl prefab = effectDataEntry.effectPrefab;
        Vector3 targetPos = target.transform.position;

        // 1. patterns[0] 위치에 이펙트 생성
        List<EffectControl> effects = new();
        foreach (int location in patterns[0].values)
        {
            Vector3 pos = dirs[location] * offset;
            if (location <= 9)
                pos += targetPos;

            EffectControl effect = Instantiate(prefab);
            effect.transform.position = pos;
            effects.Add(effect);
        }

        // 2. patterns[1..N-2] 위치로 이동, durations[0..N-3] 시간
        List<UniTask> tasks = new();
        for (int i = 1; i < patterns.Count - 1; i++)
        {
            tasks.Clear();
            for (int j = 0; j < patterns[i].values.Count; j++)
            {
                int location = patterns[i].values[j];
                Vector3 pos = dirs[location] * offset;
                if (location <= 9)
                    pos += targetPos;

                tasks.Add(Util.MoveTo(effects[j].gameObject, pos, durations[i - 1]));
            }
            await UniTask.WhenAll(tasks);
        }

        // 3. 마지막 duration만큼 대기 후 파괴
        await UniTask.WaitForSeconds(durations[^1]);

        foreach (EffectControl effect in effects)
            Destroy(effect.gameObject);
    }
}