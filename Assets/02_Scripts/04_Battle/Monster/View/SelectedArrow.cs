using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SelectedArrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float spacing = 0.5f;

    private RectTransform rectTransform;
    private Vector2 originPos;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * spacing;
        rectTransform.anchoredPosition = originPos + new Vector2(0f, offset);
    }
}