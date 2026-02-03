using System.Collections;
using UnityEngine;


public class EffectControl : MonoBehaviour
{
    // Enemy 프리펩 중앙(0, 0)을 기준으로 고정된 위치 정의
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

    public void Play(int[,] pattern, float[] duration, Enemy target, int idx)
    {
        StartCoroutine(PlayRoutine(pattern, duration, target, idx));
    }

    public IEnumerator PlayRoutine(int[,] pattern, float[] duration, Enemy target, int idx)
    {
        Vector3 targetBasePos = target.transform.position;
        int rowCount = pattern.GetLength(0);

        // 0번 인덱스 위치로 즉시 설정
        int initialPositionIndex = pattern[0, idx];
        transform.position = CalculatePosition(initialPositionIndex, targetBasePos);

        // 패턴이 1칸이면 duration[0]만큼 유지 후 소멸
        if (rowCount <= 1)
        {
            yield return new WaitForSeconds(duration[0]);
            Destroy(gameObject);
            yield break;
        }

        // 1번 인덱스부터 이동 시작 (이동 시간은 duration 사용)
        for (int i = 1; i < rowCount; i++)
        {
            int positionIndex = pattern[i, idx];
            Vector3 targetPos = CalculatePosition(positionIndex, targetBasePos);

            int durationIndex = Mathf.Min(i - 1, duration.Length - 1);
            float moveDuration = duration[durationIndex];

            yield return StartCoroutine(SmoothMove(targetPos, moveDuration));
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 위치 인덱스를 기반으로 실제 월드 좌표를 계산
    /// </summary>
    private Vector3 CalculatePosition(int positionIndex, Vector3 targetBasePos)
    {
        Vector3 position;
        if (positionIndex == 10)
            position = new Vector3(0f, 6.0f, targetBasePos.z - 0.1f);
        else if (positionIndex == 11)
            position = new Vector3(0f, 0.3f, targetBasePos.z - 0.1f);
        else
        {
            position = targetBasePos + FixedOffset[positionIndex];
            position.z = targetBasePos.z - 0.1f;
        }
        return position;
    }

    /// <summary>
    /// 현재 위치에서 목표 위치로 부드럽게 이동하는 코루틴
    /// </summary>
    private IEnumerator SmoothMove(Vector3 targetPos, float duration)
    {
        if (duration <= 0f)
        {
            transform.position = targetPos;
            yield break;
        }

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
