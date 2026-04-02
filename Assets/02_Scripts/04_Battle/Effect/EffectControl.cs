using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EffectControl : MonoBehaviour
{
    private static readonly Vector3[] FixedOffset = new Vector3[12]
    {
        new Vector3(0f, 1.8f, 0f),      // 0번: 머리 위
        new Vector3(-0.8f, 1.5f, 0f),   // 1번: 좌상단
        new Vector3(0f, 1.5f, 0f),      // 2번: 상단 중앙
        new Vector3(0.8f, 1.5f, 0f),    // 3번: 우상단
        new Vector3(-0.8f, 0f, 0f),     // 4번: 좌중앙
        new Vector3(0f, 0f, 0f),        // 5번: 정중앙 
        new Vector3(0.8f, 0f, 0f),      // 6번: 우중앙
        new Vector3(-0.8f, -1.5f, 0f),  // 7번: 좌하단
        new Vector3(0f, -1.5f, 0f),     // 8번: 하단 중앙
        new Vector3(0.8f, -1.5f, 0f),   // 9번: 우하단
        new Vector3(0f, 0.8f, 0f),      // 10번: 머리와 상단 중간
        new Vector3(0f, 0.3f, 0f)      // 11번: 발밑
    };

    private CancellationTokenSource cancellationTokenSource;

    public async UniTask Play(int[,] pattern, float[] duration, Actor target, int idx)
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
        await PlayAsync(pattern, duration, target, idx, cancellationTokenSource.Token);
    }

    private async UniTask PlayAsync(int[,] pattern, float[] duration, Actor target, int idx, CancellationToken token)
    {
        Vector3 targetBasePos = target.transform.position;
        float targetZ = targetBasePos.z - 0.1f;
        int rowCount = pattern.GetLength(0);

        int initialPositionIndex = pattern[0, idx];
        transform.position = CalculatePosition(initialPositionIndex, targetBasePos, targetZ);

        if (rowCount <= 1)
        {
            await UniTask.WaitForSeconds(duration[0], cancellationToken: token);
            Destroy(gameObject);
            return;
        }

        for (int i = 1; i < rowCount; i++)
        {
            int positionIndex = pattern[i, idx];
            Vector3 targetPos = CalculatePosition(positionIndex, targetBasePos, targetZ);

            float moveDuration = i - 1 < duration.Length ? duration[i - 1] : duration[^1];
            await SmoothMoveAsync(targetPos, moveDuration, token);

            if (token.IsCancellationRequested)
                return;
        }

        Destroy(gameObject);
    }

    private Vector3 CalculatePosition(int positionIndex, Vector3 targetBasePos, float targetZ)
    {
        Vector3 position = positionIndex switch
        {
            10 => new Vector3(targetBasePos.x, 6.0f, targetZ),
            11 => new Vector3(targetBasePos.x, 0.3f, targetZ),
            _ => targetBasePos + FixedOffset[positionIndex] + new Vector3(0, 0, targetZ - targetBasePos.z)
        };
        return position;
    }

    private async UniTask SmoothMoveAsync(Vector3 targetPos, float duration, CancellationToken token)
    {
        if (duration <= 0f)
        {
            transform.position = targetPos;
            return;
        }

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            
            elapsed += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);
        }

        transform.position = targetPos;
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = null;
    }
}
