using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// [View] 화면 연출을 담당합니다. 데이터 로직은 모릅니다.
/// - 최적화: TMP의 maxVisibleCharacters를 사용하여 타자기 효과 시 GC 발생을 최소화했습니다.
/// </summary>
public class EventUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image illustrationImage;       
    [SerializeField] private TMP_Text npcDialogueText;      
    [SerializeField] private TMP_Text bottomScriptText;     
    [SerializeField] private Transform choiceContainer;     
    [SerializeField] private GameObject choiceButtonPrefab; 

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;     

    private Coroutine typingCoroutine;
    private WaitForSeconds typingWait; // 코루틴 최적화용 캐싱

    private void Awake()
    {
        typingWait = new WaitForSeconds(typingSpeed);
    }

    // UI 초기화 (이미지, 대사)
    public void SetupUI(string dialogue, string imgName)
    {
        npcDialogueText.text = dialogue;
        bottomScriptText.text = ""; 

        // 이미지 처리
        if (!string.IsNullOrEmpty(imgName) && imgName != "None")
        {
            // Resources.Load는 무겁지만 이벤트 진입 시 1회만 호출되므로 허용
            Sprite spr = Resources.Load<Sprite>($"NPC/{imgName.Trim()}"); 
            illustrationImage.sprite = spr;
            illustrationImage.gameObject.SetActive(spr != null);
        }
        else
        {
            illustrationImage.gameObject.SetActive(false);
        }
    }

    // 타자기 효과 시작
    public void PlayTypeWriter(string text, System.Action onComplete)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeWriterOptimized(text, onComplete));
    }

    // [최적화] TMP의 속성을 이용한 가비지 프리 타자기 효과
    private IEnumerator TypeWriterOptimized(string fullText, System.Action onComplete)
    {
        // 텍스트를 미리 다 세팅하고, 보이는 글자 수만 0부터 늘려나감
        bottomScriptText.text = fullText.Replace("\\n", "\n"); // 줄바꿈 처리
        bottomScriptText.maxVisibleCharacters = 0;

        int totalChars = bottomScriptText.textInfo.characterCount; // TMP가 계산한 문자 수
        // *주의: textInfo를 갱신하려면 ForceMeshUpdate가 필요할 수 있음
        bottomScriptText.ForceMeshUpdate();
        totalChars = bottomScriptText.textInfo.characterCount;

        for (int i = 0; i <= totalChars; i++)
        {
            bottomScriptText.maxVisibleCharacters = i;

            // 클릭 시 스킵 로직
            if (Input.GetMouseButtonDown(0))
            {
                bottomScriptText.maxVisibleCharacters = totalChars;
                yield return null;
                break;
            }

            yield return typingWait;
        }

        // 출력 완료 후 클릭 대기
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        onComplete?.Invoke();
    }

    // 버튼 생성 (데이터 객체 전달)
    public void CreateButton(EventChoice.ChoiceInfo choiceData, EventResult.ResultInfo rewardData, System.Action<int> onComplete)
    {
        GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
        // 버튼 스크립트에게 데이터 주입
        btnObj.GetComponent<EventButtonUI>().Setup(choiceData, rewardData, onComplete);
    }

    public void ClearButtons()
    {
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);
    }
}