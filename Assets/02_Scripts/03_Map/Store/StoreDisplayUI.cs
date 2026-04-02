using System.Collections.Generic;
using UnityEngine;

public class StoreDisplayUI : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField] private Transform displayArea;
    [SerializeField] private int targetCount = 6; // 상점에 진열할 개수
    

    [Header("Card Prefab Settings")]
    [SerializeField] private GameObject cardPrefab;

    [Header("Purchase Popup Settings")]
    [SerializeField] private PurchasePopup purchasePopup;

    private readonly List<StoreCardUI> spawnedCardList = new();
    private bool isInitialized = false;

    private void Awake()
    {
        if (purchasePopup == null)
            purchasePopup = FindFirstObjectByType<PurchasePopup>(FindObjectsInactive.Include);

        for (int i = 0; i < targetCount; i++)
        {
            GameObject newObj = Instantiate(cardPrefab, displayArea);
            spawnedCardList.Add(newObj.GetComponent<StoreCardUI>());
            newObj.SetActive(false);
        }
    }

    private void Start()
    {
        isInitialized = true;
        SetupStore();
    }

    private void OnEnable()
    {
        if (!isInitialized) return;
        SetupStore();
    }

    public void SetupStore()
    {
        // entries에 등록된 카드만 상점 판매카드로 사용
        List<CardName> cardList = new(CardManager.Instance.GetAllCardNames());
        cardList.RemoveAll(cardName => cardName.ToString().EndsWith("End"));        
        int usedCount = 0;

        for (int i = 0; i < targetCount; i++)
        {
            if (cardList.Count == 0) break;

            CardDataEntry cardData = PickRandomCard(cardList);
            StoreCardUI storeCard = spawnedCardList[i];

            storeCard.SetCardDataEntry(cardData);
            storeCard.BindPopup(purchasePopup);
            storeCard.gameObject.SetActive(true);
            usedCount++;
        }

        for (int i = usedCount; i < spawnedCardList.Count; i++)
            spawnedCardList[i].gameObject.SetActive(false);
    }

    private CardDataEntry PickRandomCard(List<CardName> cardList)
    {
        int randIndex = Random.Range(0, cardList.Count);
        CardName cardName = cardList[randIndex];

        (cardList[randIndex], cardList[^1]) = (cardList[^1], cardList[randIndex]);
        cardList.RemoveAt(cardList.Count - 1);

        return CardManager.Instance.GetCardData(cardName);
    }

    public void CloseStoreDisplay()
    {
        gameObject.SetActive(false);
    }
}