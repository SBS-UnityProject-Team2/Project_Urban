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
        UICard uiCardPrefab = cardPrefab != null ? cardPrefab.GetComponent<UICard>() : null;        

        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (DeckCard cardInstance in deckToRender)
        {
            UICard spawnedCard = Instantiate(uiCardPrefab, displayArea);

            if (cardInstance.IsEnchanted)
            {
                CardDataEntry enchantedData = CardManager.Instance.GetEnchantCardData(cardInstance.Name);
                Sprite enchantedImage = CardManager.Instance.GetEnchantCardImage(cardInstance.Name);
                spawnedCard.Init(enchantedData, enchantedImage);
            }
            else
            {
                spawnedCard.Init(cardInstance);
            }

            spawnedCard.Init(cardInstance);
            spawnedCard.transform.localScale = Vector3.one;

            Button cardButton = spawnedCard.GetComponent<Button>();            

            DeckCard capturedCard = cardInstance;
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => removeConfirmPopup.OpenPopup(capturedCard));
        }
    }
}