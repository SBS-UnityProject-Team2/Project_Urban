using Cysharp.Threading.Tasks;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 originScale;

    private static CardController selectedCard;
    public static bool IsDragging => selectedCard != null;

    private System.Func<Actor, UniTask> handleDrop;

    public void Init(System.Func<Actor, UniTask> handleDrop)
    {
        this.handleDrop = handleDrop;
    }

    private void Update()
    {
        if (selectedCard != this) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = -5.0f;
        transform.position = mouseWorldPos;

        if (Input.GetMouseButtonUp(0))
            Drop();
    }

    private void OnMouseEnter()
    {
        if (IsDragging) return;
        Highlight();
    }

    private void OnMouseExit()
    {
        if (IsDragging) return;
        Reset();
    }

    private void OnMouseDown()
    {
        selectedCard = this;
    }

    private void Drop()
    {
        selectedCard = null;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        // 카드에 타겟을 확인한다

        if (hit != null && hit.TryGetComponent(out Monster monster))
        {
            UseCard(monster).Forget();
            return;
        }

        Reset();
    }

    private async UniTaskVoid UseCard(Actor target)
    {
        if (handleDrop != null)
            await handleDrop(target);
    }

    private void Highlight()
    {
        originScale = transform.localScale;
        originPos = transform.localPosition;

        transform.localScale *= 1.2f;
        transform.localPosition = originPos + Vector3.back;
    }

    public void Reset()
    {
        transform.localScale = originScale;
        transform.localPosition = originPos;
    }
}