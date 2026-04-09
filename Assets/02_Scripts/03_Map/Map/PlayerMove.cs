using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    private CancellationTokenSource moveCts = new CancellationTokenSource();

    // 현재 이동 중인지 확인하는 변수
    public bool IsMoving { get; private set; } = false;

    // 그냥 위치만 옮겨주는 함수
    public void MoveTo(Vector3 targetPos)
    {
        MoveToAsync(targetPos).Forget();
    }


    public async UniTask MoveToAsync(Vector3 targetPos)
    {
        if (moveCts != null)
        {
            moveCts.Cancel();
            moveCts.Dispose();
        }
        moveCts = new CancellationTokenSource();
        var currentMoveCts = moveCts;
        CancellationToken token = currentMoveCts.Token;

        IsMoving = true;

        // 도착할 때까지 반복
        while (!token.IsCancellationRequested && Vector3.Distance(transform.localPosition, targetPos) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // 취소되지 않았으면 정확한 위치로 고정
        if (!token.IsCancellationRequested)
        {
            transform.localPosition = targetPos;
        }

        if (moveCts == currentMoveCts)
        {
            IsMoving = false;
        }
    }

    private void OnDestroy()
    {
        if (moveCts != null)
        {
            moveCts.Cancel();
            moveCts.Dispose();
        }
    }
}



    