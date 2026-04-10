using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardEnchantPanel : MonoBehaviour
{

    [Header("Display Settings")]
    [SerializeField] private Transform displayArea;   // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;   // 카드 슬롯 프리팹
    [SerializeField] private GameObject EnchantCardPanel; // 인챈트 패널
    [SerializeField] private GameObject emptyEnchantableMessageObject; // 강화 가능 카드가 없을 때 안내 텍스트
    

    [Header("Popup Settings")]
    [SerializeField] private EnchantConfirmPopup deckEnchantPopup;
    [SerializeField] private Button openPanelButton;

    private void OnEnable()
    {
        UpdateOpenButton();
    }

    public void OpenDeckDisplay()
    {
        if (!MapManager.Instance.CanEnchant)
        {
            UpdateOpenButton();
            return;
        }

        List<DeckCard> receivedDeck = DeckManager.Instance.Deck;
        bool hasEnchantableCard = receivedDeck.Exists(card => !card.IsEnchanted);

        RenderDeck(receivedDeck);

        if (!hasEnchantableCard)
        {
            emptyEnchantableMessageObject.SetActive(true);
            SetSpawnedCardsActive(false);
        }

        EnchantCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        EnchantCardPanel.SetActive(false);
        emptyEnchantableMessageObject.SetActive(false);
        UpdateOpenButton();
    }

    public void UpdateOpenButton()
    {
        openPanelButton.interactable = MapManager.Instance.CanEnchant;
    }

    private void RenderDeck(List<DeckCard> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (DeckCard cardInstance in deckToRender)
        {
            UICard spawnedCard = Instantiate(cardPrefab, displayArea).GetComponent<UICard>();

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

    private void SetSpawnedCardsActive(bool isActive)
    {
        foreach (Transform child in displayArea)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}