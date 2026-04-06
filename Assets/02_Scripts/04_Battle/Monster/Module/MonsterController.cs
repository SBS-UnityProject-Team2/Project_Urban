
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
        if (other.gameObject.CompareTag("Card"))
        {
            view.Select();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Card"))
        {
            view.UnSelect();
        }
    }
}