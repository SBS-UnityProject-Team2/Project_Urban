using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Reference Settings")]
    [SerializeField] private Image effectIcon;
    [SerializeField] private TMP_Text effectName;
    [SerializeField] private TMP_Text effectStack;
    [SerializeField] private StatusEffectDescPanel descPanel;

    [Header("Setting")]
    [SerializeField] private bool isPlayer;

    public void Init(StatusEffect statusEffect)
    {
        StatusEffectDataEntry dataEntry = statusEffect.Date;
        int stack = statusEffect.StatusNumber;

        if (isPlayer) effectName.text = dataEntry.koreanName;

        effectIcon.sprite = dataEntry.buffIcon;
        effectIcon.color = dataEntry.color;
        effectStack.text = stack == 0 ? "∞" : stack.ToString();

        if (isPlayer)
        {
            descPanel.Init(dataEntry);
            descPanel.gameObject.SetActive(false);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPlayer) return;

        descPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPlayer) return;

        descPanel.gameObject.SetActive(false);
    }

    public void SetStack(int stack)
    {
        effectStack.text = stack.ToString();
    }
}