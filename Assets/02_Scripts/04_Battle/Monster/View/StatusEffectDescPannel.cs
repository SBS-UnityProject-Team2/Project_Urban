using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectDescPanel : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text effectTitle;
    [SerializeField] private TMP_Text effectDesc;

    public void Init(StatusEffectDataEntry effectData)
    {
        icon.sprite = effectData.buffIcon;
        icon.color = effectData.color;
        effectTitle.text = effectData.koreanName;
        effectDesc.text = effectData.description;
    }
}