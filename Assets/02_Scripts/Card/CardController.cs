using UnityEngine;

public class CardController : MonoBehaviour
{
    static private CardController selectedCard;

    private Vector3 originPos;
    private Vector3 originScale;

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
        originScale = transform.localScale;
        originPos = transform.localPosition;

        transform.localScale *= 1.2f;
        transform.localPosition = originPos + Vector3.back;
    }

    private void OnMouseExit()
    {
        transform.localScale = originScale;
        transform.localPosition = originPos;
    }

    private void OnMouseDown()
    {
        selectedCard = this;
    }

    private void OnMouseUp()
    {
        transform.localPosition = originPos;
        transform.localScale = originScale;
        
        selectedCard = null;
    }
}