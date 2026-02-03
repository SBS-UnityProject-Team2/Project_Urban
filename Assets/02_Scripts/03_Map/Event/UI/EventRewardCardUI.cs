using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventRewardCardUI : MonoBehaviour
{
    [SerializeField] private UICard cardImage;
    [SerializeField] private RectTransform viewPoint;
    [SerializeField] private RectTransform deckPoint;

    [Header("Timings")]
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private float fadeDuration = 0.4f;

    private RectTransform cardRect;
    private Image cardGraphic;

    private void Awake()
    {
        cardRect = cardImage.GetComponent<RectTransform>();
        cardGraphic = cardImage.GetComponent<Image>();
    }

    public void PlayAddCardAnim(CardName cardName)
    {
        gameObject.SetActive(true);
        SetCardData(cardName);

        cardRect.position = viewPoint.position;
        cardRect.localScale = Vector3.one;

        StartCoroutine(AddCardRoutine());
    }

    public void PlayRemoveCardAnim(CardName cardName)
    {
        gameObject.SetActive(true);
        SetCardData(cardName);
        
        cardRect.position = deckPoint.position;
        cardRect.localScale = Vector3.zero;

        StartCoroutine(RemoveCardRoutine());
    }

    private IEnumerator WaitForClick()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }

    private IEnumerator AddCardRoutine()
    {
        yield return FadeRoutine(0.0f, 1.0f);
        yield return WaitForClick();

        Vector3 start = viewPoint.position;
        Vector3 end = deckPoint.position;

        yield return MoveRoutine(start, end, Vector3.one, Vector3.zero);

        gameObject.SetActive(false);
    }

    private IEnumerator RemoveCardRoutine()
    {
        Vector3 start = deckPoint.position;
        Vector3 end = viewPoint.position;

        SetAlpha(1.0f);

        yield return MoveRoutine(start, end, Vector3.zero, Vector3.one);
        yield return WaitForClick();
        yield return FadeRoutine(1.0f, 0.0f);

        gameObject.SetActive(false);
    }

    private IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, Vector3 startScale, Vector3 endScale)
    {
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float eased = Mathf.Sin(t * Mathf.PI * 0.5f);
            cardRect.position = Vector3.Lerp(startPos, endPos, eased);
            cardRect.localScale = Vector3.Lerp(startScale, endScale, eased);
            yield return null;
        }

        cardRect.position = endPos;
        cardRect.localScale = endScale;
    }

    private IEnumerator FadeRoutine(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float eased = Mathf.Sin(t * Mathf.PI * 0.5f);
            SetAlpha(Mathf.Lerp(from, to, eased));
            yield return null;
        }

        SetAlpha(to);
    }

    private void SetCardData(CardName cardName)
    {
        CardDataEntry cardDataEntry = CardManager.Instance.GetCardData(cardName);
        cardImage.SetCardDataEntry(cardDataEntry);
    }

    private void SetAlpha(float alpha)
    {
        Color color = cardGraphic.color;
        color.a = alpha;
        cardGraphic.color = color;
    }
}