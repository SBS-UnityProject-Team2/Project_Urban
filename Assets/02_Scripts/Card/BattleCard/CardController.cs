using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CardController : MonoBehaviour
{
    static private CardController selectedCard;

    private Vector3 originPos;
    private Vector3 originScale;
    private bool isDrag;

    private System.Func<Actor, UniTask> handleDrop;

    public void Init(System.Func<Actor, UniTask> handleDrop)
    {
        this.handleDrop = handleDrop; 
    }

    private void Update()
    {
        if (selectedCard == this)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = -5.0f;

            transform.position = mouseWorldPos;
        }
    }

    private void OnMouseEnter()
    {
        if (isDrag) return;

        originScale = transform.localScale;
        originPos = transform.localPosition;

        transform.localScale *= 1.2f;
        transform.localPosition = originPos + Vector3.back;
    }

    private void OnMouseExit()
    {
        if (isDrag) return;

        transform.localScale = originScale;
        transform.localPosition = originPos;
    }

    private void OnMouseDown()
    {
        if (isDrag) return;

        selectedCard = this;
        isDrag = true;
    }

    private async void OnMouseUp()
    {
        if (!isDrag) return;

        transform.localPosition = originPos;
        transform.localScale = originScale;
        selectedCard = null;
        
        // await handleDrop?.Invoke()
        await UniTask.WaitForSeconds(2.0f);
    }
}