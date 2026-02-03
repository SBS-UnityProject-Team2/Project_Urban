using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextActionView : MonoBehaviour
{
    [SerializeField] private Image iconImage; 
    [SerializeField] private TMP_Text damageText;

    public void UpdateNextAction(EnemyAction enemyAction)
    {
        iconImage.sprite = EnemyManager.Instance.GetActionIcon(enemyAction.Type);

        if ((enemyAction.Type & ActionType.Attack) != 0)
        {
            AttackAction attackAction = enemyAction as AttackAction;

            damageText.gameObject.SetActive(true);
            damageText.text = $"{attackAction.Damage} X {attackAction.Count}";
        }
        else
        {
            damageText.gameObject.SetActive(false); 
        }
    }
}