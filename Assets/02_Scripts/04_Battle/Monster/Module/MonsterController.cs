
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterView view;

    public void Init(MonsterView monsterView)
    {
        view = monsterView;
    }

    private void OnMouseEnter()
    {
        if (!CardController.IsDragging) return;
    }

    private void OnMouseExit()
    {
        if (!CardController.IsDragging) return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Card")) return;
        if (CardController.CurTarget == CardTarget.Self) return;

        if (CardController.CurTarget == CardTarget.MonsterAll)
        {
            foreach (Monster monster in Battle.Instance.Monsters.List)
                monster.View.Select();
        }
        else
        {
            view.Select();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Card")) return;
        if (CardController.CurTarget == CardTarget.Self) return;

        if (CardController.CurTarget == CardTarget.MonsterAll)
        {
            foreach (Monster monster in Battle.Instance.Monsters.List)
                monster.View.UnSelect();
        }
        else
        {
            view.UnSelect();
        }
    }
}