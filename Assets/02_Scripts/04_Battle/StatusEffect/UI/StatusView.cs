using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Content Settings")]
    [SerializeField] private RectTransform content;
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    [Header("Only Enemy")]
    [SerializeField] private RectTransform descContent;
    [SerializeField] private StatusEffectDescPanel descPanelPrefab;

    private IEnumerable<StatusEffect> effects;

    public void UpdateView(IEnumerable<StatusEffect> effects)
    {
        this.effects = effects;

        foreach (RectTransform child in content)
            Destroy(child.gameObject);

        foreach (var effect in effects.ToList())
            CreateStatusEffect(effect);
    }

    private void CreateStatusEffect(StatusEffect statusEffect)
    {
        StatusEffectUI statusEffectUI = Instantiate(statusEffectUIPrefab, content);
        statusEffectUI.Init(statusEffect);
    }

    private void CreateDescPanel(StatusEffectDataEntry dataEntry)
    {
        StatusEffectDescPanel descPanel = Instantiate(descPanelPrefab, descContent);
        descPanel.Init(dataEntry); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descContent == null) return;
        if (effects == null) return;

        foreach (var effect in effects.ToList())
            CreateDescPanel(effect.Date);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descContent == null) return;

        foreach (RectTransform child in descContent)
            Destroy(child.gameObject);
    }

    public void Bind(Status status)
    {
        Debug.Log("Bind 호출됨");
        status.OnUpdate.AddListener(UpdateView);
        UpdateView(status.GetActiveEffects());
    }
}