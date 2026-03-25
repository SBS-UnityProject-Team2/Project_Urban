using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private UICard uiCardPrefab;      
    private readonly List<UICard> spawnedCards = new();

    public void Display(List<CardName> cardNames, Transform targetContent)      // 어떤 카드리스트를, 어디에 까지 받아옴
    {
        int cardCount = cardNames.Count;
        CheckCount(cardCount, targetContent);

        for (int i = 0; i < cardCount; i++)
        {
            UICard spawnedCard = spawnedCards[i];
            spawnedCard.transform.SetParent(targetContent, false);
            spawnedCard.gameObject.SetActive(true);
            spawnedCard.SetCardName(cardNames[i]);           
        }

        for (int i = cardCount; i < spawnedCards.Count; i++)
            spawnedCards[i].gameObject.SetActive(false);
    }

    // 필요한 카드 프리펩 갯수만큼 만들어놓고 부족하면 추가생성, 남으면 비활성화로 대기
    private void CheckCount(int neededCount, Transform targetContent)
    {
        for (int i = spawnedCards.Count; i < neededCount; i++)
        {
            UICard spawnedCard = Instantiate(uiCardPrefab, targetContent);
            spawnedCard.gameObject.SetActive(false);
            spawnedCards.Add(spawnedCard);
        }
    }
}