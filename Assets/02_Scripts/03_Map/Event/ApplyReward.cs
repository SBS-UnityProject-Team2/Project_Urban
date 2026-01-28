// using UnityEngine;

// /// <summary>
// /// EventResult JSON에서 받은 보상 데이터를 적용하는 전용 유틸리티 클래스.
// /// (조건/선택지와 무관, 순수 보상 계산만 담당)
// /// </summary>
// public static class ApplyReward
// {
//     /// <summary>
//     /// JSON 보상 데이터 적용 (0 값이면 변화 없음)
//     /// </summary>
//     /// <param name="excludeCardSelection">카드 선택 보상을 제외할지 여부 (true면 HP/골드만 적용)</param>
//     public static void ApplyRewardFromData(EventResult.ResultInfo rewardData, bool excludeCardSelection = false)
//     {
//         ApplyHpFromData(rewardData);
            
//         ApplyGoldFromData(rewardData);
        
//         if (!excludeCardSelection)
//         {
//             ApplyCardFromData(rewardData);
//         }
//     }

//     // ========== JSON 데이터 기반 방식 ==========

//     private static void ApplyHpFromData(EventResult.ResultInfo rewardData)
//     {
//         HealthController health = PlayerManager.Instance.Health;
//         int beforeHp = health.CurrentHp;
//         float val = (health.CurrentHp * rewardData.ResultHpPresent) +
//                     (health.MaxHp * rewardData.ResultHpMaximum);
//         int amount = (int)val;

//         if (amount < 0 && amount > -4) amount = -4;

//         if (amount < 0) health.DecreaseHp(Mathf.Abs(amount));
//         else if (amount > 0) health.IncreaseHp(amount);
//     }

//     private static void ApplyGoldFromData(EventResult.ResultInfo rewardData)
//     {
//         float variance = rewardData.ResultGold * 0.2f;
//         int finalGold = (int)Random.Range(rewardData.ResultGold - variance, rewardData.ResultGold + variance);

//         if (finalGold > 0) PlayerManager.Instance.Coin.Increase(finalGold);
//         else if (finalGold < 0) PlayerManager.Instance.Coin.Decrease(Mathf.Abs(finalGold));
//     }

//     private static void ApplyCardFromData(EventResult.ResultInfo rewardData)
//     {
//         CardData cardData = EventManager.Instance.GetCardData();

//         // 결과 정의에 맞게 카드 처리 (필요시 Element 매핑 확장)

//         // 랜덤 카드 획득 (JSON 값 1,2,3,4를 Element enum으로 변환)
//         if (rewardData.ResultRandomCard > 0)
//         {
//             Element element = ConvertToElement(rewardData.ResultRandomCard);
//             Debug.Log($"[ApplyReward] ResultRandomCard = {rewardData.ResultRandomCard} -> Element = {element}");
//             EventCardReward.AddRandomCard(element, cardData);
//         }

//         // 카드 풀에서 선택하여 획득 (ResultRangeCard가 1 이상이면 카드 풀 사용)
//         if (rewardData.ResultRangeCard > 0)
//         {
//             EventCardReward.ShowCardPoolSelectionUI(rewardData.ResultRangeCard, cardData);
//         }

//         // 카드 제거 (JSON 값 1,2,3,4를 Element enum으로 변환)
//         if (rewardData.ResultRemove > 0)
//         {
//             Element element = ConvertToElement(rewardData.ResultRemove);
//             EventCardReward.RemoveRandomCard(element);
//         }
//     }

//     /// <summary>
//     /// JSON의 간단한 숫자(1,2,3,4)를 Element enum(1000,2000,3000,4000)으로 변환
//     /// </summary>
//     private static Element ConvertToElement(int value)
//     {
//         return value switch
//         {
//             1 => Element.None,    // 1 -> 1000
//             2 => Element.Ruin,    // 2 -> 2000
//             3 => Element.Psychic, // 3 -> 3000
//             4 => Element.Bio,     // 4 -> 4000
//             _ => Element.None
//         };
//     }
// }
