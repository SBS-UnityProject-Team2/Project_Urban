using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private UICard uiCardPrefab;      
    private readonly List<UICard> spawnedCards = new();

    public void Display(List<DeckCard> cardInstances, Transform targetContent)      // 어떤 카드리스트를, 어디에 까지 받아옴
    {
        int cardCount = cardInstances.Count;
        CheckCount(cardCount - spawnedCards.Count, targetContent);
            
        for (int i = 0; i < cardCount; i++)
        {
            if (i < cardCount)
                spawnedCards[i].Init(cardInstances[i]);  
            
            spawnedCards[i].gameObject.SetActive(i < cardCount);
        }
    }

    // 필요한 카드 프리펩 갯수만큼 만들어놓고 부족하면 추가생성, 남으면 비활성화로 대기
    private void CheckCount(int neededCount, Transform targetContent)
    {
        for (int i = 0; i < neededCount; i++)
        {
            UICard spawnedCard = Instantiate(uiCardPrefab, targetContent);
            spawnedCards.Add(spawnedCard);
        }
    }
}