using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("Effect Data")]
    [SerializeField] private EffectData effectData;

    /// <summary>
    /// EffectType 코드를 받아서 해당 이펙트 데이터를 반환
    /// </summary>
    public EffectDataEntry GetEffectData(EffectType effectType)
    {
        if (effectData == null) return null;
        return effectData.GetEffectData(effectType);
    }

    /// <summary>
    /// 이펙트 프리펩을 반환
    /// </summary>
    public EffectControl GetEffectPrefab(EffectType effectType) => effectData.GetEffectPrefab(effectType);
   

    /// <summary>
    /// 위치 인덱스를 실제 월드 좌표로 변환
    /// </summary>
    public Vector3 GetPositionByIndex(int positionIndex)
    {
        // TODO: 실제 게임 내 위치 매핑 로직 구현
        return Vector3.zero;
    }
}
