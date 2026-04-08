using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStatusEffectItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text valueText;

    [SerializeField] private RectTransform descPanel;
    [SerializeField] private Image decsIcon;
    [SerializeField] private TMP_Text descTitle;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private float fadeDuration = 0.15f;

    private CanvasGroup descCanvasGroup;
    private CancellationTokenSource fadeCts;

    public void Init(StatusEffect statusEffect)
    {
        StatusEffectDataEntry dataEntry = statusEffect.Date;

        itemIcon.sprite = decsIcon.sprite = dataEntry.buffIcon;
        itemIcon.color = decsIcon.color = dataEntry.color;
        nameText.text = descTitle.text = dataEntry.koreanName;
        valueText.text = statusEffect.StatusNumber.ToString();
        descText.text = dataEntry.description;

        descCanvasGroup = descPanel.GetComponent<CanvasGroup>();
        descCanvasGroup.alpha = 0f;
        descCanvasGroup.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Fade(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Fade(0f);
    }

    private void Fade(float targetAlpha)
    {
        fadeCts?.Cancel();
        fadeCts = new CancellationTokenSource();
        FadeAsync(targetAlpha, fadeCts.Token).Forget();
    }

    private async UniTaskVoid FadeAsync(float targetAlpha, CancellationToken token)
    {
        float startAlpha = descCanvasGroup.alpha;
        float curTime = 0f;

        descCanvasGroup.blocksRaycasts = targetAlpha > 0f;

        while (curTime < fadeDuration)
        {
            if (token.IsCancellationRequested) return;

            curTime += Time.deltaTime;
            descCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curTime / fadeDuration);
            await UniTask.Yield();
        }

        descCanvasGroup.alpha = targetAlpha;
    }
}