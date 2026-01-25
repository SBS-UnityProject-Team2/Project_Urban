using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Panel_Script에 붙는 스크립트
/// EventUI로부터 데이터를 받아서 NPC 대사, 상황 설명, 일러스트를 출력
/// </summary>
public class EventScriptText : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image illustrationImage;      // NPC 일러스트
    [SerializeField] private TMP_Text npcDialogueText;    // NPC 대사
    [SerializeField] private TMP_Text eventScriptText;    // 상황 설명 (Player 시점)

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private WaitForSeconds typingWait;

    private void Awake()
    {
        typingWait = new WaitForSeconds(typingSpeed);
    }

    /// <summary>
    /// EventUI로부터 받은 데이터로 UI 세팅
    /// </summary>
    public void SetupScript(string dialogue, string eventScript, string illustrationName)
    {
        npcDialogueText.text = dialogue;
        eventScriptText.text = "";
        LoadIllustration(illustrationName);

        // 상황 설명 타이핑 시작
        if (!string.IsNullOrEmpty(eventScript))
        {
            // 안전장치: GameObject가 비활성화되어 있으면 활성화
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(TypeWriterRoutine(eventScript));
        }
    }

    /// <summary>
    /// 결과 스크립트 표시 (선택 후)
    /// </summary>
    public void SetupResultScript(string dialogue, string resultScript, string endScript = "")
    {
        npcDialogueText.text = dialogue;

        // 결과 스크립트 + EndScript 결합
        string fullText = resultScript;
        if (!string.IsNullOrEmpty(endScript))
        {
            fullText += "\n\n" + endScript;
        }

        // 결과 스크립트 타이핑
        if (!string.IsNullOrEmpty(fullText))
        {
            // 안전장치: GameObject가 비활성화되어 있으면 활성화
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(TypeWriterRoutine(fullText));
        }
    }

    /// <summary>
    /// 일러스트 로드
    /// </summary>
    private void LoadIllustration(string imgName)
    {
        if (!string.IsNullOrEmpty(imgName) && imgName != "None")
        {
            Sprite spr = Resources.Load<Sprite>($"NPC/{imgName.Trim()}");
            illustrationImage.sprite = spr;
            illustrationImage.gameObject.SetActive(spr != null);
        }
        else
        {
            illustrationImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 줄 단위로 텍스트 타이핑 효과
    /// </summary>
    private IEnumerator TypeWriterRoutine(string fullText)
    {
        string processedText = fullText.Replace("\\n", "\n");
        string[] lines = processedText.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line.Trim()))
                continue;

            // 한 글자씩 타이핑
            eventScriptText.text = "";
            for (int i = 0; i < line.Length; i++)
            {
                eventScriptText.text = line.Substring(0, i + 1);

                // 클릭하면 즉시 전체 줄 표시
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    eventScriptText.text = line;
                    break;
                }

                yield return typingWait;
            }

            // 다음 줄로 넘어가기 전 클릭 대기
            yield return null;
            yield return new WaitUntil(() => 
                Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)
            );
            yield return null;
        }

        // 모든 줄 완료 후 콜백 호출 (선택지 표시 트리거)
        EventUI.Instance?.OnScriptComplete();
    }

    /// <summary>
    /// 타이핑 스킵 (즉시 전체 텍스트 표시)
    /// </summary>
    public void SkipTyping(string fullText)
    {
        StopAllCoroutines();
        eventScriptText.text = fullText.Replace("\\n", "\n");
    }
}
