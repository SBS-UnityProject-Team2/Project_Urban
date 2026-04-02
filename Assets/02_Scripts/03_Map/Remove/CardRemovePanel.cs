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
    [Header("Popup Settings")]
    [SerializeField] private RemoveConfirmPopup removeConfirmPopup;

    private int prevCardCount;

    public void OpenDeckDisplay()
    {
        List<DeckCard> receivedDeck = DeckManager.Instance.Deck;

        RenderDeck(receivedDeck);

        RemoveCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        RemoveCardPanel.SetActive(false);
    }

    private void RenderDeck(List<DeckCard> deckToRender)
    {
        UICard uiCardPrefab = cardPrefab != null ? cardPrefab.GetComponent<UICard>() : null;        

        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (DeckCard cardInstance in deckToRender)
        {
            UICard spawnedCard = Instantiate(uiCardPrefab, displayArea);
            spawnedCard.Init(cardInstance);
            spawnedCard.transform.localScale = Vector3.one;

            Button cardButton = spawnedCard.GetComponent<Button>();            

            DeckCard capturedCard = cardInstance;
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => removeConfirmPopup.OpenPopup(capturedCard));
        }
    }
}