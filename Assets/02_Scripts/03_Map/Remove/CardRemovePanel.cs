using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardRemovePanel : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField] private Transform displayArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;   // 카드 슬롯 프리팹
    [SerializeField] private GameObject RemoveCardPanel; // 패널
    [SerializeField] private GameObject emptyDeckMessage; 
    [Header("Popup Settings")]
    [SerializeField] private RemoveConfirmPopup removeConfirmPopup;

    private int prevCardCount;

    public void OpenDeckDisplay()
    {
        List<DeckCard> receivedDeck = DeckManager.Instance.Deck;

        RenderDeck(receivedDeck);
        emptyDeckMessage.SetActive(receivedDeck.Count == 0);

        RemoveCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        RemoveCardPanel.SetActive(false);
        emptyDeckMessage.SetActive(false);
    }

    private void RenderDeck(List<DeckCard> deckToRender)
    {
        UICard uiCardPrefab = cardPrefab.GetComponent<UICard>();
        CardDisplay.Display(deckToRender, displayArea, uiCardPrefab);

        for (int i = 0; i < deckToRender.Count; i++)
        {
            UICard spawnedCard = displayArea.GetChild(i).GetComponent<UICard>();
            spawnedCard.transform.localScale = Vector3.one;

            Button cardButton = spawnedCard.GetComponent<Button>();

            DeckCard capturedCard = deckToRender[i];
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => removeConfirmPopup.OpenPopup(capturedCard));
        }
    }
}