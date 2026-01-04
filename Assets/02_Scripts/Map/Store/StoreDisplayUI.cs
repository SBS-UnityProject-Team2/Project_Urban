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

    private void Awake()
    {
        for (int i = 0; i < targetCount; i++)
        {
            GameObject newObj = Instantiate(cardPrefab, displayArea);
            newObj.transform.localScale = Vector3.one;
            spawnedCardList.Add(newObj.GetComponent<StoreCardUI>());
            newObj.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SetupStore();
    }

    public void SetupStore()
    {
        // 1. 이번에 사용할 카드 리스트 준비
        List<CardName> cardList = new(CardManager.Instance.GetAllCardNames());

        for (int i = 0; i < targetCount; i++)
        {
            if (cardList.Count == 0) break;

            CardDataEntry cardData = PickRandomCard(cardList);
            StoreCardUI storeCard = spawnedCardList[i];

            storeCard.SetCardDataEntry(cardData);
            storeCard.AddClickHandler(() => purchasePopup.OpenPopup(storeCard, cardData));
            storeCard.gameObject.SetActive(true);
        }
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