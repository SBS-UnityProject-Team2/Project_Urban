// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Michsky.UI.Dark;

// /// <summary>
// /// Panel_SelectCardReward에 붙는 스크립트
// /// ResultRangeCard 번호를 받아서 카드 풀의 3장을 버튼에 표시
// /// 선택한 카드를 덱에 추가
// /// </summary>
// [RequireComponent(typeof(ModalWindowManager))]
// public class EventSelectCardReward : MonoBehaviour
// {
//     [Header("Card Buttons")]
//     [SerializeField] private ButtonSelectCardReward button1;
//     [SerializeField] private ButtonSelectCardReward button2;
//     [SerializeField] private ButtonSelectCardReward button3;

//     [Header("Confirm Button")]
//     [SerializeField] private Button confirmButton;

//     private CardName selectedCard;
//     private System.Action<CardName> onCompleteCallback;

//     private void Awake()
//     {
//         // 확인 버튼 클릭 이벤트
//         confirmButton.onClick.RemoveAllListeners();
//         confirmButton.onClick.AddListener(OnConfirmClicked);
//         confirmButton.interactable = false; // 초기에는 비활성화
//     }

//     /// <summary>
//     /// ResultRangeCard 번호로 카드 풀 표시
//     /// </summary>
//     public void ShowCardPool(int poolIndex, System.Action<CardName> onComplete = null)
//     {
//         onCompleteCallback = onComplete;

//         var eventResultSO = GetEventResultSO();

//         var pool = eventResultSO.GetCardPool(poolIndex);
//         if (pool == null || !pool.IsValid())
//         {
//             Debug.LogError($"[EventSelectCardReward] 카드 풀 {poolIndex}가 유효하지 않습니다. 3장의 카드가 모두 설정되어 있는지 확인하세요.");
//             ClosePanel();
//             onCompleteCallback?.Invoke((CardName)0);
//             return;
//         }

//         List<CardName> cardList = pool.GetCardList();

//         // 패널 활성화 (ModalWindowManager 사용)
//         GetComponent<ModalWindowManager>().ModalWindowIn();

//         // 버튼들에 카드 정보 설정
//         ButtonSelectCardReward[] buttons = { button1, button2, button3 };
//         for (int i = 0; i < 3; i++)
//         {
//             if (i < cardList.Count)
//             {
//                 buttons[i].SetupCard(cardList[i], this);
//                 buttons[i].gameObject.SetActive(true);
//             }
//             else
//             {
//                 buttons[i].gameObject.SetActive(false);
//             }
//         }

//         // 선택 초기화
//         selectedCard = (CardName)0;
//         confirmButton.interactable = false;
//     }

//     /// <summary>
//     /// 카드 선택 (ButtonSelectCardReward에서 호출)
//     /// </summary>
//     public void SelectCard(CardName cardName, ButtonSelectCardReward clickedButton)
//     {
//         selectedCard = cardName;

//         // 모든 버튼의 선택 상태 업데이트
//         ButtonSelectCardReward[] buttons = { button1, button2, button3 };
//         foreach (var btn in buttons)
//         {
//             if (btn.gameObject.activeSelf)
//                 btn.SetSelected(btn == clickedButton);
//         }

//         // 확인 버튼 활성화
//         confirmButton.interactable = true;
//     }

//     /// <summary>
//     /// 확인 버튼 클릭
//     /// </summary>
//     private void OnConfirmClicked()
//     {
//         if (selectedCard == (CardName)0)
//             return;

//         // 덱에 카드 추가
//         DeckManager.Instance.AddCard(selectedCard);

//         // 콜백 호출 먼저 (버튼 비활성화 등)
//         onCompleteCallback?.Invoke(selectedCard);

//         // 패널 닫기 (애니메이션)
//         ClosePanel();
//     }

//     /// <summary>
//     /// 패널 닫기
//     /// </summary>
//     private void ClosePanel()
//     {
//         GetComponent<ModalWindowManager>().ModalWindowOut();
//     }

//     /// <summary>
//     /// EventResult SO 가져오기
//     /// </summary>
//     private EventResult GetEventResultSO()
//     {
//         var field = typeof(EventManager).GetField("eventResultSO",
//             System.Reflection.BindingFlags.NonPublic |
//             System.Reflection.BindingFlags.Instance);

//         return field.GetValue(EventManager.Instance) as EventResult;
//     }
// }
