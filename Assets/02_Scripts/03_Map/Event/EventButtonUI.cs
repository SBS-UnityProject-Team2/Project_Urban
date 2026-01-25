using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EventButtonUI : MonoBehaviour
{
    public TMP_Text choiceTitleText;
    public TMP_Text hoverRewardText;
    public Button button;
    public Image buttonImage; // 배경 이미지

    private EventChoice.ChoiceInfo myData;
    private EventUI eventUI; // EventUI 참조

    public void Setup(int choiceCode, EventUI ui)
    {
        eventUI = ui;

        // choiceCode가 0이면 비활성화
        if (choiceCode == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // EventManager에서 선택지 데이터 조회
        myData = EventManager.Instance.GetChoiceInfo(choiceCode);

        // 버튼 활성화
        gameObject.SetActive(true);

        // EventManager에서 보상 데이터 가져오기 (플레이스홀더 치환용)
        var rewardData = EventManager.Instance.GetRewardInfo(myData.ResultCode);

        // 1. 텍스트 설정 (플레이스홀더 치환)
        choiceTitleText.text = RewardTextFormatter.FormatChoiceText(myData.ChoiceName, rewardData);
        hoverRewardText.text = RewardTextFormatter.FormatChoiceText(myData.ChoiceResult, rewardData);

        // 2. 버튼 스스로 조건 확인 (ConditionCheck 유틸리티 사용)
        bool isSelectable = ConditionCheck.CheckCondition(myData.ConditionEnum);

        // 3. 카드 제거 보상이 있다면 해당 Element 카드가 있는지 추가 확인
        if (isSelectable && rewardData.ResultRemove > 0)
        {
            Element removeElement = (Element)rewardData.ResultRemove;
            bool hasCardToRemove = DeckManager.Instance.CardList.Any(card => card.Element == removeElement);
            
            if (!hasCardToRemove)
            {
                isSelectable = false;
            }
        }

        // 4. 활성화 상태 반영 (조건 불만족 시 비활성화)
        button.interactable = isSelectable;

        // 5. 시각적 처리 (회색/흰색)
        buttonImage.color = isSelectable ? Color.white : Color.gray;
        choiceTitleText.alpha = isSelectable ? 1.0f : 0.5f;

        // 6. 클릭 이벤트 연결 (보상 적용 포함)
        button.onClick.RemoveAllListeners();
        if (isSelectable)
            button.onClick.AddListener(() => OnButtonClicked());
    }

    /// <summary>
    /// 버튼 클릭 시 보상 적용 후 결과 스크립트 표시
    /// </summary>
    private void OnButtonClicked()
    {
        // 버튼 클릭 즉시 모든 선택지 버튼 비활성화
        HideAllButtons();

        var rewardData = EventManager.Instance.GetRewardInfo(myData.ResultCode);

        // 카드 풀 선택 보상인지 확인 (ResultRangeCard는 카드 풀에서 선택)
        bool hasCardPoolSelection = rewardData.ResultRangeCard > 0;

        if (hasCardPoolSelection)
        {
            // 카드 풀 선택 보상: 선택 완료 후 결과 표시
            ApplyRewardWithCardSelection(rewardData);
        }
        else
        {
            // 일반 보상 (랜덤 카드 포함): 즉시 적용 후 결과 표시
            ApplyRewardToPlayer(rewardData);
            ShowResultScript();
        }
    }

    /// <summary>
    /// 모든 버튼 비활성화 (선택 후)
    /// </summary>
    private void HideAllButtons()
    { // Panel_Choice 자체를 비활성화하여 모든 버튼 완전히 숨김       
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 결과 스크립트 표시
    /// </summary>
    private void ShowResultScript()
    {
        eventUI.ShowResultScript(myData.ScriptCode, myData.ResultCode, "");
    }

    private void ShowResultScriptWithCard(CardName selectedCard)
    {
        eventUI.ShowResultScript(myData.ScriptCode, myData.ResultCode, selectedCard.ToString());
    }

    /// <summary>
    /// 이 버튼의 선택지에 해당하는 보상을 계산하고 적용 (일반 보상, 랜덤 카드 포함)
    /// </summary>
    private void ApplyRewardToPlayer(EventResult.ResultInfo rewardData)
    {
        // excludeCardSelection: false로 설정하여 ResultRandomCard 처리 포함
        ApplyReward.ApplyRewardFromData(rewardData, excludeCardSelection: false);
        
        // 보상 요약 로그
        System.Text.StringBuilder rewardLog = new System.Text.StringBuilder("[EventButtonUI] 보상 적용 완료: ");
        bool hasReward = false;
        
        if (rewardData.ResultHpPresent != 0 || rewardData.ResultHpMaximum != 0)
        {
            rewardLog.Append("HP 변화, ");
            hasReward = true;
        }
        if (rewardData.ResultGold != 0)
        {
            rewardLog.Append($"골드 {(rewardData.ResultGold > 0 ? "+" : "")}{rewardData.ResultGold}, ");
            hasReward = true;
        }
        // ResultRandomCard 로그는 AddRandomCard에서 정확한 카드명으로 표시
        
        if (!hasReward && rewardData.ResultRandomCard == 0)
            rewardLog.Append("보상 없음");
        else
            rewardLog.Length -= 2; // 마지막 ", " 제거
    }

    /// <summary>
    /// 카드 선택 보상 처리 (선택 완료 후 결과 표시)
    /// </summary>
    private void ApplyRewardWithCardSelection(EventResult.ResultInfo rewardData)
    {
        // HP/골드 보상 먼저 적용
        ApplyReward.ApplyRewardFromData(rewardData, excludeCardSelection: true);
        
        CardData cardData = EventManager.Instance.GetCardData();

        if (rewardData.ResultRangeCard > 0)
        {
            EventCardReward.ShowCardPoolSelectionUI(
                rewardData.ResultRangeCard,
                cardData,
                onComplete: (selectedCard) => 
                {
                    ShowResultScriptWithCard(selectedCard);
                }
            );
        }
        else
        {
            ShowResultScript();
        }
    }
}