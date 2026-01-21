using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerText : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TMP_Text playerText;       // 플레이어 대사가 출력될 텍스트 컴포넌트
    [SerializeField] private GameObject selectPanel;    // 대화가 끝나면 켜질 선택지 패널 

    [Header("대사 설정")]
    [SerializeField] private float typingSpeed = 0.05f; // 글자 나오는 속도
    
    // 일단 미리 텍스트 직접입력
    [TextArea(3, 5)]
    [SerializeField] private string[] sentences = new string[]
    {
        "로브를 걸친 사람이 절뚝거리며 다가와 말을 걸어온다.",
        "한번도 본 적 없는 인상인데…",
        "미심쩍지만 거래에 응하는 것도 나쁘지 않을 것 같다는 생각이 들었다."
    };

    private int index = 0;
    private bool isTyping = false;
    private string currentSentence = "";

    // 이 패널이 켜질 때(SetActive true)마다 자동으로 시작됨
    private void OnEnable()
    {
        index = 0;
        
        // 시작할 때 선택지 패널은 꺼둠
        selectPanel.SetActive(false);
        
        // 첫 문장 시작
        StartCoroutine(TypeSentence());
    }

    private void Update()
    {
        // 클릭 또는 스페이스바 감지
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // 1. 타이핑 중이라면 -> 즉시 전체 문장 보여주기 (스킵기능)
        if (isTyping)
        {
            StopAllCoroutines();
            playerText.text = currentSentence;
            isTyping = false;
        }
        // 2. 타이핑이 끝난 상태라면 -> 다음 문장으로 넘어가거나 종료
        else
        {
            NextSentence();
        }
    }

    private void NextSentence()
    {
        index++;

        // 아직 보여줄 문장이 남았다면
        if (index < sentences.Length)
        {
            StartCoroutine(TypeSentence());
        }
        else
        {
            // 모든 문장이 끝났음 -> 선택지 패널 활성화!
            
            
            selectPanel.SetActive(true);   
            
            this.enabled = false; // 더 이상 입력 안 받음
        }
    }

    private IEnumerator TypeSentence()
    {
        isTyping = true;
        currentSentence = sentences[index];
        playerText.text = "";

        foreach (char letter in currentSentence.ToCharArray())
        {
            playerText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}