using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (!CardController.IsDragging) return;
    }

    private void OnMouseExit()
    {
        if (!CardController.IsDragging) return;
    }
}