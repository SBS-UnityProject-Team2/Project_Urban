// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;

// /// <summary>
// /// 이벤트 카드 보상 전용 유틸리티 클래스
// /// (카드 추가, 제거, 선택 처리)
// /// </summary>
// public static class EventCardReward
// {
//     /// <summary>
//     /// 카드 풀에서 3장 중 1장 선택 (ResultRangeCard 사용)
//     /// </summary>
//     public static void ShowCardPoolSelectionUI(int poolIndex, CardData cardDataSO, System.Action<CardName> onComplete = null)
//     {
//         // EventManager에서 패널 참조 가져오기
//         EventSelectCardReward selectPanel = EventManager.Instance.GetCardRewardPanel();
        
//         // Inspector에 연결 안 되어있으면 FindAnyObjectByType으로 찾기 (fallback)
//         selectPanel = Object.FindAnyObjectByType<EventSelectCardReward>(FindObjectsInactive.Include);
        
//         selectPanel.ShowCardPool(poolIndex, onComplete);
//     }

//     /// <summary>
//     /// EventManager에서 EventResult ScriptableObject 가져오기
//     /// </summary>
//     private static EventResult GetEventResultSO()
//     {
//         // Reflection으로 private SerializedField 접근
//         var field = typeof(EventManager).GetField("eventResultSO", 
//             System.Reflection.BindingFlags.NonPublic | 
//             System.Reflection.BindingFlags.Instance);
        
//         return field.GetValue(EventManager.Instance) as EventResult;
//     }

//     /// <summary>
//     /// 3장 중 1장 선택 (카드 ID로)
//     /// ResultInfo의 RewardCard1/2/3을 받아서 선택 UI 표시
//     /// </summary>
//     /// <param name="onComplete">카드 선택 완료 후 호출될 콜백</param>
//     public static void ShowCardSelectionUI(int card1Id, int card2Id, int card3Id, CardData cardDataSO, System.Action onComplete = null)
//     {

//         // 카드 ID를 CardName으로 변환
//         List<CardName> cardOptions = new List<CardName>();
        
//         if (card1Id > 0) cardOptions.Add((CardName)card1Id);
//         if (card2Id > 0) cardOptions.Add((CardName)card2Id);
//         if (card3Id > 0) cardOptions.Add((CardName)card3Id);

//         if (cardOptions.Count == 0)
//         {
//             onComplete?.Invoke();
//             return;
//         }

//         // 카드 선택 UI 표시 (CardReward 시스템 재활용)
//         CardSelectUI cardSelectUI = Object.FindFirstObjectByType<CardSelectUI>();
//         // CardSelectUI에 콜백 전달
//         cardSelectUI.Initialize((selectedCard) => 
//         {
//             DeckManager.Instance.AddCard(selectedCard);
//             onComplete?.Invoke();
//         });
//         cardSelectUI.gameObject.SetActive(true);
//     }

//     /// <summary>
//     /// 카드 선택 처리 (후보 목록에서 첫 번째 카드를 덱에 추가)
//     /// </summary>
//     public static void ProcessCardSelection(List<CardName> candidates)
//     {
//         if (candidates == null || candidates.Count == 0) 
//             return;

//         CardName selectedCard = candidates[0];
//         DeckManager.Instance.AddCard(selectedCard);
//     }

//     /// <summary>
//     /// 특정 속성의 카드를 CardData에서 랜덤 선택하여 덱에 추가
//     /// [최적화] GetCardsByElement()로 필터링된 목록만 순회
//     /// </summary>
//     public static void AddRandomCard(Element element, CardData cardDataSO)
//     {
//         List<CardDataEntry> pool = cardDataSO.GetCardsByElement(element);

//         if (pool == null || pool.Count == 0)
//         {
//             return;
//         }

//         CardDataEntry selectedCardData = pool[Random.Range(0, pool.Count)];
//         CardName randomCard = selectedCardData.cardName;
//         DeckManager.Instance.AddCard(randomCard);
//     }

//     /// <summary>
//     /// 특정 속성의 카드를 덱에서 랜덤 선택하여 제거
//     /// [최적화] LINQ Where로 필터링 후 Random 선택 (불필요한 순회 방지)
//     /// </summary>
//     public static void RemoveRandomCard(Element element)
//     {
//         List<Card> targetCards = DeckManager.Instance.CardList
//             .Where(c => c.Element == element)
//             .ToList();

//         if (targetCards.Count > 0)
//         {
//             Card cardToRemove = targetCards[Random.Range(0, targetCards.Count)];
//             DeckManager.Instance.RemoveCard(cardToRemove);
//             UnityEngine.Object.Destroy(cardToRemove.gameObject);
//         }
//     }
// }
