using System.Collections.Generic;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    public void UpdateView(IEnumerable<StatusEffect> effects)
    {
        int idx = 0;

        foreach (var effect in effects)
            CreateStatusEffect(effect, idx++);
    }

    private void CreateStatusEffect(StatusEffect statusEffect, int idx)
    {
        StatusEffectUI statusEffectUI = Instantiate(statusEffectUIPrefab, content);
        statusEffectUI.Init(null, statusEffect.Name.ToString(), statusEffect.StatusNumber);
        statusEffectUI.gameObject.transform.position = new Vector3(0 ,-100 * idx, 0);
    }

    public void Bind(Status status)
    {
        Debug.Log("Bind 호출됨");
        status.OnUpdate.AddListener(UpdateView);
        UpdateView(status.GetActiveEffects());
    }
}