using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : SceneSingleton<EffectManager>
{
    [SerializeField] private EffectData effectData;
    [SerializeField] private CanvasGroup playerHitEffect;
    [SerializeField] private float fadeDuration = 0.15f;
    private CancellationTokenSource fadeCts;

    public EffectDataEntry GetEffectData(EffectType effectType)
    {
        return effectData.GetEffectData(effectType);
    }

    public async UniTask PlayerHitEffect()
    {
        playerHitEffect.alpha = 1.0f;
        playerHitEffect.gameObject.SetActive(true);

        fadeCts?.Cancel();
        fadeCts = new CancellationTokenSource();

        // await FadeAsync(1, fadeCts.Token);
        await FadeAsync(0, fadeCts.Token);

        playerHitEffect.gameObject.SetActive(false);

    }

    private async UniTask FadeAsync(float targetAlpha, CancellationToken token)
    {
        float startAlpha = playerHitEffect.alpha;
        float curTime = 0f;

        playerHitEffect.blocksRaycasts = targetAlpha > 0f;

        while (curTime < fadeDuration)
        {
            if (token.IsCancellationRequested) return;

            curTime += Time.deltaTime;
            playerHitEffect.alpha = Mathf.Lerp(startAlpha, targetAlpha, curTime / fadeDuration);
            await UniTask.Yield();
        }

        playerHitEffect.alpha = targetAlpha;
    }
}