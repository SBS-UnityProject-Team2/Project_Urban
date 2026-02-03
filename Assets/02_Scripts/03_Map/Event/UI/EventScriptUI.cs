using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EventScriptUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image illustrationImage;       // NPC 일러스트
    [SerializeField] private TMP_Text playerScriptText;     // 상황 설명 (Player 시점)
    [SerializeField] private TMP_Text npcScriptText;        // NPC 대사
    [SerializeField] private TMP_Text resultScriptText;     // 보상
    [SerializeField] private Image resultScriptPanel;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

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

    public void Init()
    {
        isSkip = false;
        playerScriptText.text = string.Empty;
        npcScriptText.text = string.Empty;
        resultScriptText.text = string.Empty;

        resultScriptPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void StartEventScript(int scriptCode, UnityAction onComplete = null)
    {
        EventScript eventScript = EventManager.Instance.GetEventScript(scriptCode);
        StartCoroutine(EventScriptRoutine(eventScript, onComplete));
    }

    private IEnumerator EventScriptRoutine(EventScript eventScript, UnityAction onComplete = null)
    {
        yield return PrintScriptRoutine(eventScript.playerScript, playerScriptText);

        yield return new WaitUntil(IsAdvanceInputPressed);
        yield return null;

        yield return PrintScriptRoutine(eventScript.npcDialogue, npcScriptText);

        onComplete?.Invoke();
    }

    public void StartEndScript(int scriptCode, string resultString, UnityAction onComplete = null)
    {
        resultScriptPanel.gameObject.SetActive(true);

        EventScript eventScript = EventManager.Instance.GetEventScript(scriptCode);
        StartCoroutine(EndScriptRoutine(eventScript, resultString, onComplete));
    }

    private IEnumerator EndScriptRoutine(EventScript eventScript, string resultString, UnityAction onComplete = null)
    {
        yield return PrintScriptRoutine(eventScript.npcDialogue, npcScriptText);

        yield return new WaitUntil(IsAdvanceInputPressed);
        yield return null;

        yield return PrintScriptRoutine(eventScript.playerScript, playerScriptText);

        yield return new WaitUntil(IsAdvanceInputPressed);
        yield return null;

        yield return PrintScriptRoutine(resultString, resultScriptText);

        onComplete?.Invoke();
    }

    private IEnumerator PrintScriptRoutine(string[] scripts, TMP_Text textUI)
    {
        isSkip = false;
        int length = scripts.Length;

        for (int i = 0; i < length; i++)
        {
            char[] charArray = scripts[i].ToCharArray();
            int charCount = scripts[i].Length;

            int idx = 0;
            while (idx < charCount)
            {
                if (isSkip)
                {
                    textUI.SetCharArray(charArray, 0, charCount);

                    break;
                }

                idx = GetNextIndex(charArray, idx);
                textUI.SetCharArray(charArray, 0, idx);

                yield return typingWait;
            }

            yield return null;

            if (i < length - 1)
            {
                yield return new WaitUntil(IsAdvanceInputPressed);
                textUI.text = string.Empty;
                yield return null;
            }

            isSkip = false;
        }
    }

    private IEnumerator PrintScriptRoutine(string script, TMP_Text textUI)
    {
        isSkip = false;

        char[] charArray = script.ToCharArray();
        int charCount = script.Length;

        int idx = 0;
        while (idx < charCount)
        {
            if (isSkip)
            {
                textUI.SetCharArray(charArray, 0, charCount);

                break;
            }

            textUI.SetCharArray(charArray, 0, ++idx);

            yield return typingWait;
        }

        yield return null;

        isSkip = false;
    }

    // TODO : Input System으로 변경하기
    private bool IsAdvanceInputPressed()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }

    private int GetNextIndex(char[] charArray, int startIdx)
    {
        if (charArray[startIdx] == '<')
        {
            for (int i = startIdx + 1; i < charArray.Length; i++)
                if (charArray[i] == '>') return i + 1;
        }

        return startIdx + 1;
    }
}
