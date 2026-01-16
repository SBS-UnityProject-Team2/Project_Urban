using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    [Header("Content Settings")]
    [SerializeField] private Transform content;
    [SerializeField] private float startYPosition;
    [SerializeField] private float spacing;
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    public void UpdateView(IEnumerable<StatusEffect> effects)
    {
        int idx = 0;

        foreach (var effect in effects.ToList())
            CreateStatusEffect(effect, idx++);
    }

    private void CreateStatusEffect(StatusEffect statusEffect, int idx)
    {
        StatusEffectUI statusEffectUI = Instantiate(statusEffectUIPrefab, content);
        float yPos = -spacing * idx + startYPosition;

        statusEffectUI.Init(null, statusEffect.Name.ToString(), statusEffect.StatusNumber);
        statusEffectUI.GetComponent<RectTransform>().localPosition = new Vector3(0 , yPos, 0);
    }

    public void Bind(Status status)
    {
        Debug.Log("Bind 호출됨");
        status.OnUpdate.AddListener(UpdateView);
        UpdateView(status.GetActiveEffects());
    }
}