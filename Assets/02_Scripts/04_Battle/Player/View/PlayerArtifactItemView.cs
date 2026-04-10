using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerArtifactItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text textTitle;
    [SerializeField] private TMP_Text textDesc;
    [SerializeField] private CanvasGroup descPanel;

    private CancellationTokenSource fadeCts;

    public void Init(ArtifactId artifactId)
    {
        ArtifactInfo artifactInfo = ArtifactManager.Instance.GetInfo(artifactId);

        icon.sprite = artifactInfo.image;
        textTitle.text = artifactInfo.name;
        textDesc.text = artifactInfo.desc;

        descPanel.alpha = 0.0f;
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
        float startAlpha = descPanel.alpha;
        float curTime = 0f;

        descPanel.blocksRaycasts = targetAlpha > 0f;

        while (curTime < 0.15f)
        {
            if (token.IsCancellationRequested) return;

            curTime += Time.deltaTime;
            descPanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, curTime / 0.15f);
            await UniTask.Yield();
        }

        descPanel.alpha = targetAlpha;
    }
}