using UnityEngine;
using UnityEngine.EventSystems; // UI 클릭 감지용

public class EventButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        TriggerEvent();
    }

    // 실제 이벤트 실행 함수
    private void TriggerEvent()
    {
        
        // 만들어둔 EventManager의 랜덤 이벤트 함수 호출
        EventManager.Instance.StartRandomEvent();
        
    }
}