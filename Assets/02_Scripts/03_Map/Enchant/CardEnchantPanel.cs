using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardEnchantPanel : MonoBehaviour
{

    [Header("Display Settings")]
    [SerializeField] private Transform displayArea;   // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;   // 카드 슬롯 프리팹
    [SerializeField] private GameObject EnchantCardPanel; // 인챈트 패널

    [Header("Popup Settings")]
    [SerializeField] private EnchantConfirmPopup deckEnchantPopup;

    public void OpenDeckDisplay()
    {
        List<DeckCard> receivedDeck = DeckManager.Instance.Deck;

        RenderDeck(receivedDeck);

        EnchantCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        EnchantCardPanel.SetActive(false);
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

            spawnedCard.transform.localScale = Vector3.one;

            Button cardButton = spawnedCard.GetComponent<Button>();            

            DeckCard capturedCard = cardInstance;
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => deckEnchantPopup.OpenPopup(capturedCard));
        }
    }
}