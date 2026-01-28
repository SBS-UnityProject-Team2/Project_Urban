using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EventScriptUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image illustrationImage;     // NPC 일러스트
    [SerializeField] private TMP_Text npcDialogueText;    // NPC 대사
    [SerializeField] private TMP_Text eventScriptText;    // 상황 설명 (Player 시점)

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private EventScript eventScript;
    private WaitForSeconds typingWait;
    private bool isSkip;

    private void Awake()
    {
        typingWait = new WaitForSeconds(typingSpeed);
    }

    private void Update()
    {
        if (!isSkip && IsAdvanceInputPressed())
            isSkip = true;
    }

    public void Init(int scriptCode)
    {
        eventScript = EventManager.Instance.GetEventScript(scriptCode);
        
        isSkip = false;
        eventScriptText.text = string.Empty;
        npcDialogueText.text = string.Empty;
    }

    public void StartScript(UnityAction onComplete = null)
    {
        StartCoroutine(PrintAllScriptRoutine(onComplete));
    }

    private IEnumerator PrintAllScriptRoutine(UnityAction onComplete = null)
    {
        yield return PrintScriptRoutine(eventScript.eventScript, eventScriptText);
        yield return PrintScriptRoutine(eventScript.dialogue, npcDialogueText);

        onComplete?.Invoke();
    }

    private IEnumerator PrintScriptRoutine(string script, TMP_Text textUI)
    {
        string [] splitString = script.Split('\n');
        int length = splitString.Length;

        for (int i = 0; i < length; i++)
        {
            char [] charArray = splitString[i].ToCharArray();
            int charCount = splitString[i].Length;

            for (int j = 0; j < charCount; j++)
            {
                if (isSkip)
                {
                    textUI.SetCharArray(charArray, 0, charCount);

                    break;
                }

                textUI.SetCharArray(charArray, 0, j);   

                yield return typingWait;
            }

            yield return null;
            isSkip = false;
            
            if (i < length - 1)
            {
                textUI.text = string.Empty;
                yield return new WaitUntil(IsAdvanceInputPressed);
            }
        }
    }

    // TODO : Input System으로 변경하기
    private bool IsAdvanceInputPressed()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }
}
