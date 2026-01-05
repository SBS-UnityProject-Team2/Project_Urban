using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatusEffectListView : MonoBehaviour
{
    [SerializeField] private TMP_Text statusEffectListText;

    public void UpdateView(IEnumerable<StatusEffect> effects)
    {
        Debug.Log("UpdateView 호출됨");
        StringBuilder stringBuilder = new();
        
        foreach (var effect in effects)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Append(", ");
            stringBuilder.Append(effect.ToString());
        }
        
        statusEffectListText.text = stringBuilder.ToString();
        Debug.Log($"상태 효과 텍스트 업데이트: {stringBuilder.ToString()}");
    }

    public void Bind(StatusEffectList statusEffectList)
    {
        Debug.Log("Bind 호출됨");
        statusEffectList.OnUpdateList.AddListener(UpdateView);
        UpdateView(statusEffectList.EffectList);
    }
}