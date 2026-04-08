using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectItemView : MonoBehaviour
{
    [Header("UI Reference Settings")]
    [SerializeField] private Image effectIcon;
    [SerializeField] private TMP_Text effectStack;

    public void Init(StatusEffect statusEffect)
    {
        StatusEffectDataEntry dataEntry = statusEffect.Date;
        int stack = statusEffect.StatusNumber;

        effectIcon.sprite = dataEntry.buffIcon;
        effectIcon.color = dataEntry.color;
        effectStack.text = stack == 0 ? "∞" : stack.ToString();
    }
}