using System.Collections;
using System.Numerics;
using UnityEngine;


public class EffectControl : MonoBehaviour
{
    static readonly float [] offset =
    {
        new Vector(-5.0f, 5.0f, 0),
        new Vector(0, 5.0f, 0),
        new Vector(5.0f, 5,0f, 0),
    };

    private const float AUTO_DESTROY_DELAY = 0.5f;

    public void Play(int[,] pattern, float[] duration, Enemy target, int idx)
    {
        // 경로 [idx, 0], [idx, 1], [idx, 2]... 
        // 경로 배열 int [] = {}
        Debug.Log($"[EffectControl.Play] 시작 - target: {target?.name}");
        
        // null 체크
        if (pattern == null || duration == null || target == null)
        {
            Debug.LogWarning("[EffectControl.Play] pattern, duration 또는 target이 NULL");
            Destroy(gameObject);
            return;
        }
        
        // SpriteRenderer가 없으면 기본 위치 사용
        SpriteRenderer sprite = target.GetComponentInChildren<SpriteRenderer>();
        Vector3 basePos = sprite != null ? sprite.bounds.center : target.transform.position;
        
        Debug.Log($"[EffectControl.Play] basePos: {basePos}, sprite: {sprite?.name}");
        
        Vector3[] offsets = BuildOffsets(target);
        
        // 첫 번째 패턴 위치에 이펙트 배치
        int colCount = pattern.GetLength(1);
        
        Debug.Log($"[EffectControl.Play] pattern rows: {pattern.GetLength(0)}, cols: {colCount}, childCount: {transform.childCount}");
        
        // 자식이 있으면 자식들을 각 패턴 위치로
        if (transform.childCount > 0)
        {
            // 루트를 basePos에 두고 자식들을 오프셋으로 배치
            transform.position = basePos;
            Debug.Log($"[EffectControl.Play] 루트 위치 설정: {basePos}");
            
            for (int j = 0; j < colCount; j++)
            {
                if (j < transform.childCount)
                {
                    int index = pattern[0, j];
                    transform.GetChild(j).localPosition = offsets[index];
                    Debug.Log($"[EffectControl.Play] 자식[{j}] 위치: {offsets[index]} (index: {index})");
                }
            }
        }
        else
        {
            // 자식이 없으면 이펙트 자체를 첫 번째 패턴 위치로
            if (colCount > 0)
            {
                int index = pattern[0, 0];
                Vector3 finalPos = basePos + offsets[index];
                transform.position = finalPos;
                Debug.Log($"[EffectControl.Play] 이펙트 위치 설정: {finalPos} (basePos: {basePos}, offset: {offsets[index]}, index: {index})");
            }
        }
        
        StartCoroutine(PlayRoutine(pattern, duration, basePos, offsets));
    }

    public IEnumerator PlayRoutine(int[] path, float[] duration, Vector3 basePos, Vector3[] offsets)
    {
        // int rowCount = pattern.GetLength(0);
        // int colCount = pattern.GetLength(1);

        for (int i = 0; i < path.Length; i++)
        {
            // 이동 처리
            transform.position = basePos + offset[path];
            // 대기
            yield return new WaitForSeconds(duration[i]);
        }

        // for (int i = 0; i < rowCount; i++)
        // {
        //     // 자식이 있으면 자식들 이동, 없으면 자신 이동
        //     if (transform.childCount > 0)
        //     {
        //         for (int j = 0; j < colCount; j++)
        //         {
        //             if (j < transform.childCount)
        //             {
        //                 int index = pattern[i, j];
        //                 transform.GetChild(j).localPosition = offsets[index];
        //             }
        //         }
        //     }
        //     else
        //     {
        //         if (colCount > 0)
        //         {
        //             int index = pattern[i, 0];
        //             transform.position = basePos + offsets[index];
        //         }
        //     }

        //     if (i < duration.Length)
        //         yield return new WaitForSeconds(duration[i]);
        // }

        // 필요없음
        // yield return new WaitForSeconds(AUTO_DESTROY_DELAY);
        Destroy(gameObject);
    }

    private Vector3[] BuildOffsets(Enemy target)
    {
        SpriteRenderer sprite = target.GetComponentInChildren<SpriteRenderer>();
        Bounds bounds = sprite.bounds;

        float thirdW = bounds.size.x / 3f;
        float thirdH = bounds.size.y / 3f;

        Vector3[] offsets = new Vector3[12];

        offsets[0] = new Vector3(0f, bounds.extents.y + thirdH * 0.5f, 0f);

        int idx = 1;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                float x = (col - 1) * thirdW;
                float y = (1 - row) * thirdH;
                offsets[idx] = new Vector3(x, y, 0f);
                idx++;
            }
        }

        // 10번: 0번(머리 위)과 2번(상단 중앙)의 중간
        float pos0Y = bounds.extents.y + thirdH * 0.5f;
        float pos2Y = thirdH;  // 상단 중앙
        offsets[10] = new Vector3(0f, (pos0Y + pos2Y) / 2f, 0f);
        
        // 11번: 8번(하단 중앙)보다 조금 아래 (발밑)
        float pos8Y = -thirdH;  // 하단 중앙
        offsets[11] = new Vector3(0f, -bounds.extents.y - thirdH, 0f);

        return offsets;
    }
}
