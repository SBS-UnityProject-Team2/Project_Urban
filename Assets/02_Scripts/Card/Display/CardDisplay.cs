using System.Collections.Generic;
using UnityEngine;

public static class CardDisplay
{
    private static readonly Dictionary<Transform, List<UICard>> poolDict = new();

    public static void Display(List<DeckCard> cardInstances, Transform targetContent, UICard prefab)
    {
        if (!poolDict.ContainsKey(targetContent))
        {
            poolDict[targetContent] = new List<UICard>();
        }

        List<UICard> spawnedCards = poolDict[targetContent];
        int cardCount = cardInstances.Count;

        // 부족한 갯수만큼 카드를 추가로 생성
        int cardsToCreate = cardCount - spawnedCards.Count;
        for (int i = 0; i < cardsToCreate; i++)
        {
            // static 클래스에서는 Instantiate 앞에 Object. 을 붙여서 사용합니다.
            UICard spawnedCard = Object.Instantiate(prefab, targetContent);
            spawnedCards.Add(spawnedCard);
        }

        for (int i = 0; i < spawnedCards.Count; i++)
        {
            if (i < cardCount)
            {
                spawnedCards[i].Init(cardInstances[i]);  
                spawnedCards[i].gameObject.SetActive(true);
            }
            else
            {
                spawnedCards[i].gameObject.SetActive(false);
            }
        }
    }
}