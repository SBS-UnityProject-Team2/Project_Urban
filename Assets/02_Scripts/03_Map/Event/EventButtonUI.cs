using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EventButtonUI : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text choiceTitleText;   
    public TMP_Text hoverRewardText;   
    public Button button;              

    // 버튼이 보유한 데이터
    private EventResult.ResultInfo myRewardData; 
    private int nextScriptCode;                  
    private System.Action<int> onCompleteCallback; 

    // 초기화: 데이터 주입 및 텍스트 파싱
    public void Setup(EventChoice.ChoiceInfo choiceData, EventResult.ResultInfo rewardData, System.Action<int> onComplete)
    {
        this.myRewardData = rewardData;
        this.nextScriptCode = choiceData.ScriptCode;
        this.onCompleteCallback = onComplete;

        choiceTitleText.text = choiceData.ChoiceName;

        // 텍스트 파싱: [h], [c]를 실제 숫자로 변환
        if (rewardData != null)
        {
            hoverRewardText.text = ParseRewardText(choiceData.ChoiceResult, rewardData);
        }
        else
        {
            hoverRewardText.text = choiceData.ChoiceResult;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        // 1. 보상 지급 로직 수행
        if (myRewardData != null) ApplyReward();

        // 2. 매니저에게 완료 보고
        onCompleteCallback?.Invoke(nextScriptCode);
    }


    // 텍스트 파싱 로직 (UI 표시용)

    private string ParseRewardText(string originalText, EventResult.ResultInfo reward)
    {
        var player = BattleManager.Instance.Player;
        if (player == null) return originalText;

        string resultText = originalText;

        // [h] 처리: 예상 체력 변동값 계산
        if (resultText.Contains("[h]"))
        {
            int hpChange = CalculateHpChange(player, reward);
            // UI에는 절대값(크기)으로 표시 (예: "체력 4 피해")
            resultText = resultText.Replace("[h]", Mathf.Abs(hpChange).ToString());
        }

        // [c] 처리: 예상 코인 획득량 계산
        if (resultText.Contains("[c]"))
        {
            // 단순 표기용: 기준값 표시
            resultText = resultText.Replace("[c]", Mathf.Abs(reward.ResultGold).ToString());
        }

        return resultText;
    }


    // 실제 보상 지급 로직 (데이터 적용용)

    private void ApplyReward()
    {
        var player = BattleManager.Instance.Player;

        // 1. 체력 처리 (계산식 적용)
        if (myRewardData.ResultHpPresent != 0 || myRewardData.ResultHpMaximum != 0)
        {
            int amount = CalculateHpChange(player, myRewardData);
            
            if (amount < 0) player.Health.DecreaseHp(Mathf.Abs(amount));
            else player.Health.IncreaseHp(amount);

            Debug.Log($"[Reward] 체력 변동: {amount}");
        }

        // 2. 골드 처리 (랜덤 공식 적용: +- 20%)
        if (myRewardData.ResultGold != 0)
        {
            int finalGold = CalculateGoldChange(myRewardData.ResultGold);

            if (finalGold > 0) player.Coin.IncreaseCoin(finalGold);
            else player.Coin.DecreaseCoin(Mathf.Abs(finalGold));

            Debug.Log($"[Reward] 골드 변동: {finalGold} (기준: {myRewardData.ResultGold})");
        }

        // 3. 카드 보상
        if (myRewardData.ResultRangeCard != 0) 
            Debug.Log($"[Reward] 속성 카드 획득 (ID: {myRewardData.ResultRangeCard})");
        
        if (myRewardData.ResultRandomCard != 0) 
            Debug.Log($"[Reward] 랜덤 카드 획득 (ID: {myRewardData.ResultRandomCard})");
            
        if (myRewardData.ResultRemove != 0)
            Debug.Log($"[Reward] 카드 제거 실행");
    }


    // 공통 계산 공식 함수    
    // 체력 변동량 계산
    private int CalculateHpChange(Player player, EventResult.ResultInfo reward)
    {
        // 공식: 현재체력% + 최대체력%
        float presentVal = player.Health.CurrentHp * reward.ResultHpPresent;
        float maxVal = player.Health.MaxHp * reward.ResultHpMaximum;
        
        // 소수점 버림 (int 캐스팅)
        int totalChange = (int)(presentVal + maxVal);

        // [최소 피해량 보정]
        // 값이 음수(피해)이고, -4보다 크다면(즉 -1, -2, -3 이라면) -4로 고정
        if (totalChange < 0 && totalChange > -4)
        {
            totalChange = -4;
        }

        return totalChange;
    }

    // 골드 변동량 계산 (랜덤)
    private int CalculateGoldChange(int baseGold)
    {
        // 공식: ResultGold ± 20%
        float variance = baseGold * 0.2f;
        
        // 랜덤 범위: (기준 - 20%) ~ (기준 + 20%)
        float randomVal = Random.Range(baseGold - variance, baseGold + variance);
        
        return (int)randomVal;
    }
}