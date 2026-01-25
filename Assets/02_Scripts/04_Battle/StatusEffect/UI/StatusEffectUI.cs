using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [Header("UI Reference Settings")]
    [SerializeField] private Image effectIcon;
    [SerializeField] private TMP_Text effectName;
    [SerializeField] private TMP_Text effectStack;

    [Header("Setting")]
    [SerializeField] private bool isPlayer; 

    public void Init(StatusEffect statusEffect)
    {
        StatusEffectDataEntry dataEntry = statusEffect.Date;
        int stack = statusEffect.StatusNumber;

        if (isPlayer) effectName.text = dataEntry.koreanName;
        
        effectIcon.sprite = dataEntry.buffIcon;
        effectStack.text = stack == 0 ? "∞" : stack.ToString();
    }

    public void SetStack(int stack)
    {
        effectStack.text = stack.ToString();
    }
}