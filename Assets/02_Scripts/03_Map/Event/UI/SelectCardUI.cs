using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.Dark;
using UnityEngine.Events;


public class SelectCardUI : MonoBehaviour
{
    [Header("Card Buttons")]
    [SerializeField] private List<UICard> cards;

    [Header("Skip Button")]
    [SerializeField] private Button skipButton;

    public void Init()
    {
        foreach (UICard card in cards)
        {
            card.GetComponent<Button>().onClick.RemoveAllListeners();
            card.gameObject.SetActive(false);
        }

        skipButton.onClick.RemoveAllListeners();
    }

    public void SetSelectCards(CardName[] cardNames, UnityAction<CardName> onSelect, UnityAction onSkip)
    {
        for (int i = 0; i < cardNames.Length; i++)
        {
            CardName cardName = cardNames[i];
            CardDataEntry cardDataEntry = CardManager.Instance.GetCardData(cardName);

            cards[i].SetCardDataEntry(cardDataEntry);
            cards[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                DeckManager.Instance.AddCard(cardName);
                onSelect?.Invoke(cardName);
            });

            cards[i].gameObject.SetActive(true);
        }

        skipButton.onClick.AddListener(onSkip);
    }
}
