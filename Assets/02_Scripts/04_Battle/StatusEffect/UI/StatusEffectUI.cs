using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [Header("UI Reference Settings")]
    [SerializeField] private Image effectIcon;
    [SerializeField] private TMP_Text effectName;
    [SerializeField] private TMP_Text effectStack;

    public void Init(Sprite icon, string name, int stack)
    {
        effectIcon.sprite = icon;
        effectName.text = name;
        effectStack.text = stack == 0 ? "∞" : stack.ToString();
    }

    public void SetStack(int stack)
    {
        effectStack.text = stack.ToString();
    }
}