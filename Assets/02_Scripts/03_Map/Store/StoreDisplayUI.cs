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
    private static readonly List<CardName> cachedCardPool = BuildCardPool();

    private void Awake()
    {
        for (int i = 0; i < targetCount; i++)
        {
            GameObject newObj = Instantiate(cardPrefab, displayArea);
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
        // 캐시된 풀을 복사해서 이번 상점 후보 리스트로 사용
        List<CardName> cardList = new(cachedCardPool);
        int usedCount = 0;

        for (int i = 0; i < targetCount; i++)
        {
            if (cardList.Count == 0) break;

            CardDataEntry cardData = PickRandomCard(cardList);
            StoreCardUI storeCard = spawnedCardList[i];

            storeCard.SetCardDataEntry(cardData);
            storeCard.BindPopup(purchasePopup, cardData);
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

    private static List<CardName> BuildCardPool()
    {
        List<CardName> list = new();
        CardName[] allCardNames = (CardName[])System.Enum.GetValues(typeof(CardName));

        foreach (CardName cardName in allCardNames)
        {
            if (cardName.ToString().EndsWith("End"))
                continue;

            list.Add(cardName);
        }

        return list;
    }
}