using UnityEngine;

public class StatusEffectManager : SceneSingleton<StatusEffectManager>
{
    [SerializeField] private StatusEffectData statusEffectData;

    public StatusEffectDataEntry GetEffectData(StatusEffectName statusEffectName)
    {
        return statusEffectData.GetEffectData(statusEffectName);
    }
}