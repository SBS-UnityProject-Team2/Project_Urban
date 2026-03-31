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
        List<CardInstance> receivedDeck = DeckManager.Instance.Deck;

        RenderDeck(receivedDeck);

        EnchantCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        EnchantCardPanel.SetActive(false);
    }

    private void RenderDeck(List<CardInstance> deckToRender)
    {
        UICard uiCardPrefab = cardPrefab != null ? cardPrefab.GetComponent<UICard>() : null;        

        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (CardInstance cardInstance in deckToRender)
        {
            UICard spawnedCard = cardInstance.Instantiate(uiCardPrefab, Vector3.zero, displayArea);
            spawnedCard.transform.localScale = Vector3.one;

            Button cardButton = spawnedCard.GetComponent<Button>();            

            CardInstance capturedCard = cardInstance;
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => deckEnchantPopup.OpenPopup(capturedCard));
        }
    }
}