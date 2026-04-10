using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealingPopupUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupObject; // 팝업 패널
    [SerializeField] private TMP_Text statusText;    // 결과 텍스트 (예: 현재체력 : 100 > 500)
    [SerializeField] private Button HealButton;

    public void RefreshActionState()
    {
        HealButton.interactable = MapManager.Instance.CanEnchant;
    }

    public void OpenPopup()
    {
        popupObject.SetActive(true);

        RefreshActionState();
        bool canUseShelter = MapManager.Instance.CanEnchant;

        if (!canUseShelter)
        {
            return;
        }

        UpdateStatusText(false); // 팝업 열릴 때는 텍스트만 갱신 (회복 X)
    }

    public void ClosePopup()
    {
        popupObject.SetActive(false);
    }

    // 팝업 열릴 때 현재 상태 보여주기용 함수
    private void UpdateStatusText(bool isHealed, int beforeHp = 0)
    {
        HealthController playerHp = PlayerManager.Instance.Health;

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
        if (!MapManager.Instance.CanEnchant)
        {
            RefreshActionState();
            return;
        }

        // 1. 플레이어 HP 컨트롤러 가져오기
        HealthController playerHp = PlayerManager.Instance.Health;

        // 2. 회복 전 체력 저장
        int beforeHp = playerHp.CurrentHp;
        int maxHp = playerHp.MaxHp;

        // 3. 이미 체력이 가득 찼으면 그냥 리턴
        if (beforeHp >= maxHp)
        {
            statusText.text = "이미 체력이 가득 찼습니다.";
            return;
        }
        
        playerHp.RefillHp();

        // 쉼터 (강화/회복) 중 하나를 사용했으므로 이후 비활성화
        MapManager.Instance.SetCanEnchant(false);
        RefreshActionState();

        // 6. 팝업 텍스트 갱신 (150 > 500 형태)
        UpdateStatusText(true, beforeHp);        
    }    
}