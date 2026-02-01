using System.Collections;
using UnityEngine;


public class EffectControl : MonoBehaviour
{
    // Enemy 프리펩 중앙(0, 0)을 기준으로 고정된 위치 정의
    private static readonly Vector3[] FixedOffset = new Vector3[12]
    {
        new Vector3(0f, 1.8f, 0f),      // 0번: 머리 위
        new Vector3(-1.5f, 1.5f, 0f),   // 1번: 좌상단
        new Vector3(0f, 1.5f, 0f),      // 2번: 상단 중앙
        new Vector3(1.5f, 1.5f, 0f),    // 3번: 우상단
        new Vector3(-1.5f, 0f, 0f),     // 4번: 좌중앙
        new Vector3(0f, 0f, 0f),        // 5번: 정중앙 
        new Vector3(1.5f, 0f, 0f),      // 6번: 우중앙
        new Vector3(-1.5f, -1.5f, 0f),  // 7번: 좌하단
        new Vector3(0f, -1.5f, 0f),     // 8번: 하단 중앙
        new Vector3(1.5f, -1.5f, 0f),   // 9번: 우하단
        new Vector3(0f, 1.8f, 0f),      // 10번: 머리와 상단 중간
        new Vector3(0f, 0.3f, 0f)      // 11번: 발밑
    };

    private const float SMOOTH_MOVE_DURATION = 0.15f;

    public void Play(int[,] pattern, float[] duration, Enemy target, int idx)
    {
        Vector3 targetBasePos = target.transform.position;
        int initialPositionIndex = pattern[0, idx];
        Vector3 initialPos = targetBasePos + FixedOffset[initialPositionIndex];
        initialPos.z = targetBasePos.z - 0.1f;
        transform.position = initialPos;

        StartCoroutine(PlayRoutine(pattern, duration, targetBasePos, idx));
    }

    public IEnumerator PlayRoutine(int[,] pattern, float[] duration, Vector3 targetBasePos, int idx)
    {
        int rowCount = pattern.GetLength(0);

        for (int i = 0; i < rowCount; i++)
        {
            int positionIndex = pattern[i, idx];

            Vector3 targetPos;
            if (positionIndex == 10)
                targetPos = new Vector3(0f, 6.0f, targetBasePos.z - 0.1f);
            else if (positionIndex == 11)
                targetPos = new Vector3(0f, 0.3f, targetBasePos.z - 0.1f);
            else
            {
                targetPos = targetBasePos + FixedOffset[positionIndex];
                targetPos.z = targetBasePos.z - 0.1f;
            }

            yield return StartCoroutine(SmoothMove(targetPos, SMOOTH_MOVE_DURATION));

            if (i < duration.Length)
                yield return new WaitForSeconds(duration[i]);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// 현재 위치에서 목표 위치로 부드럽게 이동하는 코루틴
    /// </summary>
    private IEnumerator SmoothMove(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Sin 곡선으로 부드러운 Easing 적용
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // 최종 위치 확정
        transform.position = targetPos;
    }
}
