using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private CardDisplay cardDisplayPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;
    [SerializeField] private TMP_Text cardCost;
    [SerializeField] private Image cardImage;

    private readonly List<CardDisplay> spawnedDisplays = new();
    private CardName cardName;

    public CardName CardName => cardName;

    public void Display(List<Card> cards)
    {
        int cardCount = cards.Count;
        CheckCount(cardCount);        

        for (int i = 0; i < cardCount; i++)
        {
            Card card = cards[i];
            CardDisplay display = spawnedDisplays[i];
            
            display.Bind(card.CardData);
        }

        for (int i = 0; i < spawnedDisplays.Count; i++)
            spawnedDisplays[i].gameObject.SetActive(i<cardCount);
    }

    public void Bind(CardDataEntry data)
    {       
        cardName = data.cardName;
        cardImage.sprite = data.cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;
        cardCost.text = data.cost.ToString();
    }

    // 필요한 카드 프리펩 갯수만큼 만들어놓고 부족하면 추가생성, 남으면 비활성화로 대기
    private void CheckCount(int neededCount)
    {
        for (int i = spawnedDisplays.Count; i < neededCount; i++)
        {
            CardDisplay display = Instantiate(cardDisplayPrefab, content);
            display.gameObject.SetActive(false);
            spawnedDisplays.Add(display);
        }
    }
}