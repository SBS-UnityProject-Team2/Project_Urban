using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class EventScriptUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image illustrationImage;       // NPC 일러스트
    [SerializeField] private ScriptUI playerScript;     // 상황 설명 (Player 시점)
    [SerializeField] private ScriptUI npcScript;        // NPC 대사
    [SerializeField] private ScriptUI resultScript;     // 보상

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private WaitForSeconds typingWait;


    private void Awake()
    {
        typingWait = new WaitForSeconds(typingSpeed);
    }

    public void Init()
    {
        playerScript.Init();
        npcScript.Init();
        resultScript.Init();

        resultScript.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public async UniTask StartEventScript(int scriptCode)
    {
        EventScript eventScript = EventManager.Instance.GetEventScript(scriptCode);
        
        await playerScript.PrintScript(eventScript.playerScript);
        await ScriptUI.WaitClick();

        await npcScript.PrintScript(eventScript.npcDialogue);
    }

    public async UniTask StartEndScript(int scriptCode, string resultString, UnityAction onComplete = null)
    {
        resultScript.gameObject.SetActive(true);

        EventScript eventScript = EventManager.Instance.GetEventScript(scriptCode);
        
        await npcScript.PrintScript(eventScript.npcDialogue);
        await ScriptUI.WaitClick();

        await playerScript.PrintScript(eventScript.playerScript);
        await ScriptUI.WaitClick();

        await resultScript.PrintScript(resultString);
    }
}
