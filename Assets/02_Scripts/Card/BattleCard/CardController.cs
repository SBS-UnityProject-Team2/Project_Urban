using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 originScale;

    private bool isMoving;
    private CancellationTokenSource highlightCts;

    private static CardController selectedCard;
    private static CardTarget curTarget;
    public static bool IsDragging => selectedCard != null;
    public static CardTarget CurTarget => curTarget;

    private CardTarget cardTarget;
    private System.Func<Actor, UniTask> handleDrop;

    public void Init(System.Func<Actor, UniTask> handleDrop, CardTarget target)
    {
        this.handleDrop = handleDrop;
        cardTarget = target;
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
        if (!Battle.Instance.Player.IsTurn) return;
        if (isMoving) return;
        if (IsDragging) return;
        Highlight();
    }

    private void OnMouseExit()
    {
        if (!Battle.Instance.Player.IsTurn) return;
        if (isMoving) return;
        if (IsDragging) return;
        Reset();
    }

    private void OnMouseDown()
    {
        if (!Battle.Instance.Player.IsTurn) return;
        if (isMoving) return;
        
        selectedCard = this;
        curTarget = cardTarget;
    }

    private void Drop()
    {
        selectedCard = null;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int useCardZoneLayer = LayerMask.GetMask("UseCardZone");
        Collider2D zoneHit = Physics2D.OverlapPoint(mouseWorldPos, useCardZoneLayer);

        if (zoneHit == null)
        {
            Reset();
            return;
        }

        int monsterLayer = LayerMask.GetMask("Monster");
        Collider2D monsterHit = Physics2D.OverlapPoint(mouseWorldPos, monsterLayer);
        if (cardTarget == CardTarget.Self && monsterHit == null)
        {
            UseCard(Battle.Instance.Player).Forget();
            return;
        }

        if ((cardTarget == CardTarget.Monster || cardTarget == CardTarget.MonsterAll)
            && monsterHit != null && monsterHit.TryGetComponent(out Monster monster))
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
        highlightCts?.Cancel();
        highlightCts = new CancellationTokenSource();

        Vector3 targetScale = originScale * 1.2f;
        Vector3 targetPos = originPos + Vector3.back;

        AnimateAsync(targetScale, targetPos, 0.25f, highlightCts.Token).Forget();
    }

    public void Reset()
    {
        highlightCts?.Cancel();
        highlightCts = new CancellationTokenSource();

        AnimateAsync(originScale, originPos, 0.25f, highlightCts.Token).Forget();
    }

    private async UniTaskVoid AnimateAsync(Vector3 targetScale, Vector3 targetPos, float duration, CancellationToken token)
    {
        float curTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.localPosition;

        while (curTime < duration)
        {
            if (token.IsCancellationRequested) return;

            float t = Mathf.Sin((curTime / duration) * Mathf.PI * 0.5f);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);

            curTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.localScale = targetScale;
        transform.localPosition = targetPos;
    }

    public void SetMoving(bool value)
    {
        isMoving = value;
    }

    public void UpdateOrigin()
    {
        originPos = transform.localPosition;
        originScale = transform.localScale;
    }
}