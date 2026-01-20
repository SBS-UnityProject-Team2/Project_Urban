using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayCardList : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private UICard uiCardPrefab;

    [Header("UI Reference")]
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_Text titleText;

    [Header("Title Settings")]
    [SerializeField] private string deckCardListTitle = "전체 카드 목록";
    [SerializeField] private string usedCardListTitle = "사용한 카드 목록";
    [SerializeField] private string extinctCardListTitle = "소멸된 카드 목록";

    private Deck deck;
    public Deck Deck => deck ??= BattleManager.Instance.Player.Deck;

    public void ClosePanel()
    {
        gameObject.SetActive(false);

        foreach (Transform child in content)
            Destroy(child.gameObject);

        BattleManager.Instance.Restart();
    }

    public void OpenDeckCardList()
    {
        List<Card> cardList = Deck.UnusedCardList.ToList();
        cardList.AddRange(Deck.UsedCardList);
        cardList.AddRange(Deck.Hand.CurHand);

        cardList.Sort(DeckManager.Instance.CardListSortCompare);

        RenderCardList(deckCardListTitle, cardList);
    }

    public void OpenUsedCardList()
    {
        RenderCardList(usedCardListTitle, Deck.UsedCardList);
    }

    public void OpenExtinctCardList()
    {
        RenderCardList(extinctCardListTitle, Deck.ExtinctCardList);
    }

    private void RenderCardList(string title, IEnumerable<Card> cardList)
    {
        foreach (Card card in cardList)
            CreateUICard(card.Data);

        titleText.text = title;

        BattleManager.Instance.Pause();
        gameObject.SetActive(true);
    }
    
    private UICard CreateUICard(CardDataEntry cardDataEntry)
    {
        UICard uiCard = Instantiate(uiCardPrefab, content);
        uiCard.SetCardDataEntry(cardDataEntry);
        
        return uiCard;
    }
}