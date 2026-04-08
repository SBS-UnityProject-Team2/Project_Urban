using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextActionView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text damageText;

    public void UpdateView(MonsterActionDataEntry dateEntry)
    {
        image.sprite = MonsterManager.Instance.GetMonsterActionIcon(dateEntry.actionType);

        if ((dateEntry.actionType & MonsterActionType.Attack) == 0)
        {
            damageText.gameObject.SetActive(false);
            
            return;
        }
        
        damageText.gameObject.SetActive(true);
        damageText.text = $"{dateEntry.count} X {dateEntry.damage}";
    }

    public void Bind(MonsterAction monsterAction)
    {
        monsterAction.OnUpdateNextAction.AddListener(UpdateView);
    }
}