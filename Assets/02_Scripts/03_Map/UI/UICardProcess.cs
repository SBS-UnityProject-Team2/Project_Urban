using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public enum CardProcessType
{
    None,
    Shop,       
    Event,      
    ViewOnly    
}

[RequireComponent(typeof(UICard))] 
public class UICardProcess : MonoBehaviour
{
    [Header("컴포넌트 연결")]
    [SerializeField] private Button cardBtn;
    [SerializeField] private GameObject priceTagObj; 
    [SerializeField] private TMP_Text priceText;     

    private UICard uiCard;
    private CardDataEntry cardData;
    private CardProcessType processType;
    private UnityAction<CardDataEntry> onClickCallback; 
    private UnityAction onCardClickCache;

    // 상점 전용 상태 변수
    private int price;
    private bool isSoldOut;

    private void Awake()
    {
        uiCard = GetComponent<UICard>();

        if (cardBtn == null) cardBtn = GetComponentInChildren<Button>();

        if (priceTagObj == null)
        {
            Transform findObj = transform.Find("PriceText");
            if (findObj != null)
            {
                priceTagObj = findObj.gameObject;                
                priceText = findObj.GetComponentInChildren<TMP_Text>(true);
            }
        }
        onCardClickCache = OnCardClick;
    }

    public void Initialize(CardDataEntry data, CardProcessType type, UnityAction<CardDataEntry> onClickAction = null)
    {
        cardData = data;
        processType = type;
        onClickCallback = onClickAction;
        isSoldOut = false;

        // 1. 이미지/텍스트 그리기
        uiCard.SetCardDataEntry(data);

        // 2. 버튼 초기화 및 이벤트 연결
        if (cardBtn != null)
        {
            cardBtn.interactable = true;
            
            cardBtn.onClick.RemoveAllListeners();

            cardBtn.onClick.AddListener(onCardClickCache);
        }

        // 3. 타입에 따라 행동 결정 
        switch (processType)
        {
            case CardProcessType.Shop:
                SetupShopMode(); 
                break;
            
            case CardProcessType.ViewOnly:
                if (cardBtn != null) cardBtn.interactable = false;
                if (priceTagObj != null) priceTagObj.SetActive(false);
                break;
            
            default: 
                if (priceTagObj != null) priceTagObj.SetActive(false); 
                break;
        }
    }

    private void SetupShopMode()
    {
        price = cardData.price; 

        if (priceTagObj != null && priceText != null)
        {
            priceTagObj.SetActive(true);
            
            priceText.text = $"{price} G";
        }
    }

    private void OnCardClick()
    {
        if (processType == CardProcessType.Shop && isSoldOut) return;
        if (onClickCallback != null) onClickCallback.Invoke(cardData);
    }

    public bool CanBuy()
    {
        if (isSoldOut) return false;
        if (GameManager.Instance.Coin.CanBuy(price)) return false;
        return true;
    }

    public void PurchaseSuccess()
    {
        // 1. 돈 차감
        GameManager.Instance.UseCoin(price);

        // 2. 덱에 카드 추가
        DeckManager.Instance.AddCard(cardData.cardName);

        // 3. UI 매진 처리
        isSoldOut = true;
        if (cardBtn != null) cardBtn.interactable = false;
        
        if (priceTagObj != null)
        {
            priceTagObj.SetActive(true);
            if (priceText != null) priceText.text = "Sold Out";
        }
    }
}