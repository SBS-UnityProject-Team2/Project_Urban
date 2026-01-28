// using UnityEngine;

// /// <summary>
// /// 이벤트 결과 텍스트의 플레이스홀더를 실제 값으로 치환
// /// [h] -> HP 변화량
// /// [c] -> 골드 변화량
// /// [선택한 카드] -> 카드 이름
// /// </summary>
// public static class RewardTextFormatter
// {
//     /// <summary>
//     /// 선택지 텍스트의 플레이스홀더를 실제 보상 값으로 치환 (버튼 표시용)
//     /// </summary>
//     public static string FormatChoiceText(string text, EventResult.ResultInfo rewardData)
//     {

//         string result = text;

//         // [h] - HP 변화량 계산 및 치환
//         if (result.Contains("[h]"))
//         {
//             int hpChange = CalculateHpChange(rewardData);
//             result = result.Replace("[h]", Mathf.Abs(hpChange).ToString());
//         }

//         // [c] - 골드 변화량 치환
//         if (result.Contains("[c]"))
//         {
//             int goldChange = CalculateGoldChange(rewardData);
//             result = result.Replace("[c]", Mathf.Abs(goldChange).ToString());
//         }

//         return result;
//     }

//     /// <summary>
//     /// EndScript의 플레이스홀더를 실제 보상 값으로 치환
//     /// </summary>
//     public static string FormatEndScript(string endScript, EventResult.ResultInfo rewardData, string selectedCardName = "")
//     {
//         if (string.IsNullOrEmpty(endScript))
//             return endScript;

//         string result = endScript;

//         // [h] - HP 변화량 계산 및 치환
//         if (result.Contains("[h]"))
//         {
//             int hpChange = CalculateHpChange(rewardData);
//             result = result.Replace("[h]", Mathf.Abs(hpChange).ToString());
//         }

//         // [c] - 골드 변화량 치환
//         if (result.Contains("[c]"))
//         {
//             int goldChange = CalculateGoldChange(rewardData);
//             result = result.Replace("[c]", Mathf.Abs(goldChange).ToString());
//         }

//         // [선택한 카드] - 선택한 카드 이름 치환
//         if (result.Contains("[선택한 카드]"))
//         {
//             if (!string.IsNullOrEmpty(selectedCardName))
//             {
//                 string koreanName = GetKoreanCardName(selectedCardName);
//                 result = result.Replace("[선택한 카드]", koreanName);
//             }
//             else
//                 result = result.Replace("[선택한 카드]", "???");
//         }

//         return result;
//     }

//     /// <summary>
//     /// CardName enum 이름을 한글 이름으로 변환 (CardManager 통해)
//     /// </summary>
//     private static string GetKoreanCardName(string cardNameString)
//     {
//         // CardName enum 파싱
//         System.Enum.TryParse<CardName>(cardNameString, out CardName cardName);

//         // CardManager에서 카드 데이터 조회
//         CardDataEntry cardData = CardManager.Instance.GetCardData(cardName);

//         return cardData.koreanName;
//     }

//     /// <summary>
//     /// HP 변화량 계산 (ApplyReward와 동일한 로직)
//     /// </summary>
//     private static int CalculateHpChange(EventResult.ResultInfo rewardData)
//     {
//         HealthController health = PlayerManager.Instance.Health;
//         float val = (health.CurrentHp * rewardData.ResultHpPresent) +
//                     (health.MaxHp * rewardData.ResultHpMaximum);
//         int amount = (int)val;

//         if (amount < 0 && amount > -4)
//             amount = -4;

//         return amount;
//     }

//     /// <summary>
//     /// 골드 변화량 계산 (ApplyReward와 동일한 로직)
//     /// </summary>
//     private static int CalculateGoldChange(EventResult.ResultInfo rewardData)
//     {
//         float variance = rewardData.ResultGold * 0.2f;
//         int finalGold = (int)Random.Range(rewardData.ResultGold - variance, rewardData.ResultGold + variance);

//         return finalGold;
//     }
// }
