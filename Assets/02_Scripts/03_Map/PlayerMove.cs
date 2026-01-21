using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;

    // 현재 이동 중인지 확인하는 변수
    public bool IsMoving { get; private set; } = false;

    // 그냥 위치만 옮겨주는 함수
    public void MoveTo(Vector3 targetPos)
    {
        transform.localPosition = targetPos;
    }


    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        IsMoving = true;

        // 도착할 때까지 반복
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 정확한 위치로 고정
        transform.localPosition = targetPos;
        IsMoving = false;
    }
}



    