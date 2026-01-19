using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AddCardList : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private RectTransform content;
    [SerializeField] private UICard uiCardPrefab;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
       
        List<CardDataEntry> cardDataEntries = CardManager.Instance.GetAllCardData().ToList();

        foreach (CardDataEntry cardDataEntry in cardDataEntries)
        {
            CreateUICard(cardDataEntry);
        }
        
    }

    private UICard CreateUICard(CardDataEntry cardDataEntry)
    {
        UICard uiCard = Instantiate(uiCardPrefab, content);
        uiCard.SetCardDataEntry(cardDataEntry);
        
        Button button = uiCard.gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => HandleClick(cardDataEntry.cardName));

        return uiCard;
    }

    private void HandleClick(CardName cardName)
    {
        Deck.DeckCard newCard = new Deck.DeckCard(cardName, false);

        BattleManager.Instance.Player.Deck.Hand.AddCard(newCard);
    }
}