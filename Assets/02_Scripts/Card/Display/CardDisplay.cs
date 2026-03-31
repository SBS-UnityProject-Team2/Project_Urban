using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : Singleton<CardDisplay>
{
    private readonly List<UICard> spawnedCards = new();    

    public void Display(List<DeckCard> cardInstances, Transform targetContent)      // 어떤 카드리스트를, 어디에 까지 받아옴
    {
        int cardCount = cardInstances.Count;
        
        CheckCount(cardCount, prefab, targetContent);

        for (int i = 0; i < spawnedCards.Count; i++)
        {
            if (i < cardCount)
            {
                spawnedCards[i].transform.SetParent(targetContent, false);
                
                spawnedCards[i].Init(cardInstances[i]);  
                spawnedCards[i].gameObject.SetActive(true);
            }
            else
            {
                spawnedCards[i].gameObject.SetActive(false);
            }
        }
    }

    private void CheckCount(int neededCount, UICard prefab, Transform targetContent)
    {
        int cardsToCreate = neededCount - spawnedCards.Count;
        
        for (int i = 0; i < cardsToCreate; i++)
        {
            UICard spawnedCard = Instantiate(prefab, targetContent);
            spawnedCards.Add(spawnedCard);
        }
    }
}