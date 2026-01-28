// using UnityEngine;
// using UnityEngine.UI;

// /// <summary>
// /// Button_SelectCardReward에 붙는 스크립트
// /// 미리 만들어진 UICard 프리팹에 카드 데이터를 주입하여 표시
// /// </summary>
// public class ButtonSelectCardReward : MonoBehaviour
// {
//     [Header("Card UI")]
//     [SerializeField] private UICard uiCard; // 미리 배치된 UICard 프리팹
    
//     [Header("Selection Visual")]
//     [SerializeField] private GameObject selectionFrame; // 선택 표시 프레임
//     [SerializeField] private Color normalColor = Color.white;
//     [SerializeField] private Color selectedColor = Color.yellow;

//     [Header("Button")]
//     [SerializeField] private Button button;

//     private CardName myCardName;
//     private EventSelectCardReward parentPanel;

//     private void Awake()
//     {
//         // UICard가 Inspector에 연결되지 않았으면 자식에서 찾기
//         if (uiCard == null)
//         {
//             uiCard = GetComponentInChildren<UICard>(true);
//         }

//         if (button != null)
//         {
//             button.onClick.RemoveAllListeners();
//             button.onClick.AddListener(OnCardClicked);
//         }
//         SetSelected(false);
//     }

//     /// <summary>
//     /// 카드 정보 설정 (UICard에 데이터 주입)
//     /// </summary>
//     public void SetupCard(CardName cardName, EventSelectCardReward panel)
//     {
//         myCardName = cardName;
//         parentPanel = panel;

//         CardDataEntry cardData = CardManager.Instance.GetCardData(cardName);
//         if (cardData == null)
//         {
//             gameObject.SetActive(false);
//             return;
//         }

//         // UICard에 카드 데이터 주입 (텍스트 + 이미지 자동 업데이트)
//         if (uiCard != null)
//         {
//             uiCard.SetCardDataEntry(cardData);
//             uiCard.gameObject.SetActive(true);
//         }
        
//         if (button != null)
//             button.interactable = true;
//         SetSelected(false);
//     }

//     /// <summary>
//     /// 카드 버튼 클릭
//     /// </summary>
//     private void OnCardClicked()
//     {
//         parentPanel.SelectCard(myCardName, this);
//     }

//     /// <summary>
//     /// 선택 상태 시각적 표시
//     /// </summary>
//     public void SetSelected(bool isSelected)
//     {
//         if (selectionFrame != null)
//             selectionFrame.SetActive(isSelected);

//         // UICard의 이미지 색상 변경
//         if (uiCard != null)
//         {
//             Image cardImage = uiCard.GetComponent<Image>();
//             if (cardImage != null)
//                 cardImage.color = isSelected ? selectedColor : normalColor;
//         }
//     }
// }
        