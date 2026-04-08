using UnityEngine;

public class EffectManager : SceneSingleton<EffectManager>
{
    [SerializeField] private EffectData effectData;

    public EffectDataEntry GetEffectData(EffectType effectType)
    {
        return effectData.GetEffectData(effectType);
    }
}