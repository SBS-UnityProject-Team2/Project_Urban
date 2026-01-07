using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : SceneSingleton<StatusEffectManager>
{
    [SerializeField] private List<StatusEffect> statusEffectList = new();

    private readonly Dictionary<StatusEffectName, StatusEffect> statuesEffectMap = new();

    private void Start()
    {
        foreach(StatusEffect effect in statusEffectList)
            statuesEffectMap[effect.Name] = effect;
    }
}