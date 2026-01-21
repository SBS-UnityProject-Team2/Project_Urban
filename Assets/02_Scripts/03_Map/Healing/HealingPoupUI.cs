using UnityEngine;
using TMPro;

public class HealingPopupUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupObject; // 팝업 패널
    [SerializeField] private TMP_Text statusText;    // 결과 텍스트 (예: 현재체력 : 100 > 500)

    public void OpenPopup()
    {
        popupObject.SetActive(true);
        UpdateStatusText(false); // 팝업 열릴 때는 텍스트만 갱신 (회복 X)
    }

    public void ClosePopup()
    {
        popupObject.SetActive(false);
    }

    // 팝업 열릴 때 현재 상태 보여주기용 함수
    private void UpdateStatusText(bool isHealed, int beforeHp = 0)
    {
        var playerHp = GameManager.Instance.PlayerHealth;
        if (playerHp == null || statusText == null) return;

        int current = playerHp.CurrentHp;
        int max = playerHp.MaxHp;

        if (isHealed)
        {
            // 회복 후: "현재체력 : {이전} > {최대}"
            statusText.text = $"현재체력 : {beforeHp} > {max}";
        }
        else
        {
            // 회복 전: "현재체력 : {현재} / {최대}"
            statusText.text = $"현재체력 : {current} / {max}";
        }
    }

    // 버튼 연결
    public void OnClickFullHeal()
    {
        // 1. 플레이어 HP 컨트롤러 가져오기
        var playerHp = GameManager.Instance.PlayerHealth;

        // 2. 회복 전 체력 저장
        int beforeHp = playerHp.CurrentHp;
        int maxHp = playerHp.MaxHp;

        // 3. 이미 체력이 가득 찼으면 그냥 리턴
        if (beforeHp >= maxHp)
        {
            if (statusText != null) statusText.text = "이미 체력이 가득 찼습니다.";
            return;
        }
        
        playerHp.RefillHp();

        // 6. 팝업 텍스트 갱신 (150 > 500 형태)
        UpdateStatusText(true, beforeHp);        
    }

    
}