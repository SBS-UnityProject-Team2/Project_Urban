using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    [Header("Content Settings")]
    [SerializeField] private Transform content;
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    public void UpdateView(IEnumerable<StatusEffect> effects)
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (var effect in effects.ToList())
            CreateStatusEffect(effect);
    }

    private void CreateStatusEffect(StatusEffect statusEffect)
    {
        StatusEffectUI statusEffectUI = Instantiate(statusEffectUIPrefab, content);
        statusEffectUI.Init(null, statusEffect.Name.ToString(), statusEffect.StatusNumber);
    }

    public void Bind(Status status)
    {
        Debug.Log("Bind 호출됨");
        status.OnUpdate.AddListener(UpdateView);
        UpdateView(status.GetActiveEffects());
    }
}