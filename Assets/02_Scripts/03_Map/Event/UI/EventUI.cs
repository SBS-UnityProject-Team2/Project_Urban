using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ╔════════════════════════════════════════════════════════════════════╗
/// ║                    EVENT UI (컨트롤러 계층)                          ║
/// ╚════════════════════════════════════════════════════════════════════╝
/// 
/// 📋 역할:
///   • 이벤트 데이터 조회 및 전달
///   • Panel_Script와 Panel_Choice에 데이터 분배
///   • 이벤트 흐름 제어 (시작 → 스크립트 표시 → 선택지 표시 → 종료)
/// 
/// 🔄 새로운 구조:
///   EventUI (데이터 중계만)
///   ├─ Panel_Script (EventScriptText) - 스크립트/대사/일러스트 출력
///   └─ Panel_Choice (Button1, 2, 3) - 선택지 처리
/// </summary>
public class EventUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private EventScriptUI scriptUI;  // Panel_Script
    [SerializeField] private EventButtonsUI buttonsUI;

    [Header("Event Root")]
    [SerializeField] private GameObject eventPanelRoot;

    private EventInfo currentEvent;

    private void OnEnable()
    {   
        buttonsUI.gameObject.SetActive(false);

        currentEvent = EventManager.Instance.GetRandomEvent();
       
        scriptUI.Init();
        buttonsUI.Init(currentEvent.choiceCodes, scriptCode => scriptUI.StartEventScript(scriptCode));

        scriptUI.StartEventScript(currentEvent.scriptCode, () => buttonsUI.gameObject.SetActive(true));
    }
}
