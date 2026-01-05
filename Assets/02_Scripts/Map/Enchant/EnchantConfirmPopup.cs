using Michsky.UI.Dark;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(ModalWindowManager))]
public class EnchantConfirmPopup : MonoBehaviour
{
    [Header("Card UI Objects")]
    [SerializeField] private UICard BeforeCardUI; // 왼쪽 카드 프리팹 
    [SerializeField] private UICard AfterCardUI;  // 오른쪽 카드 프리팹 

    [Header("임시 강화로직")]    
    [SerializeField] private TMP_Text AfterCardNameText; 

    private ModalWindowManager mwManager;
    private CardDataEntry currentTarget; 

   



    public void OpenPopup(CardDataEntry card)
    {        
        currentTarget = card;
       
        BeforeCardUI.SetCardDataEntry(card); 
        AfterCardUI.SetCardDataEntry(card); 

        // 2. 오른쪽 카드의 이름 텍스트만 강제로 덮어쓰기
        if (AfterCardNameText != null)
        {
            AfterCardNameText.text = card.koreanName + " +";        //한글이름에 + 붙임
        }
        GetComponent<ModalWindowManager>().ModalWindowIn();
        
    }

    public void ClosePopup()
    {
        mwManager.ModalWindowOut();
    }

    public void OnClickEnchant()
    {
        Debug.Log($"[강화 성공] {currentTarget.cardName} 강화 로직 실행!");
        
        // 실제 데이터 변경은 여기서 수행
        //currentTarget.cardName = currentTarget.cardName + " +";

        ClosePopup();
    }
}