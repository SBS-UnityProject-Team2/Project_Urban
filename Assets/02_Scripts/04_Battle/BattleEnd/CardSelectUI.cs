using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectUI : MonoBehaviour
{
    [SerializeField] private RectTransform rewardUI;

    [SerializeField] private List<UICard> cardList;
    [SerializeField] private Button backButton;
    [SerializeField] private Button cardSelectButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            rewardUI.gameObject.SetActive(true);
        });

        foreach (UICard uICard in cardList)
        {
            CardDataEntry cardData = GetRandomCardData();
            uICard.Init(cardData, CardManager.Instance.GetCardImage(cardData.cardName));

            Button button = uICard.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                DeckManager.Instance.AddCard(cardData.cardName);

                gameObject.SetActive(false);
                rewardUI.gameObject.SetActive(true);

                cardSelectButton.enabled = false;
            });
        }
    }

    private CardDataEntry GetRandomCardData()
    {
        ElementType selectedType = DeckManager.Instance.SelectedElement;

        float randomValue = Random.Range(0.0f, 1.0f);
        
        CardName cardName;
        if (randomValue > 0.5f)
            cardName = CardManager.Instance.GetRandomCard(selectedType);
        else if (randomValue > 0.25f)
            cardName = CardManager.Instance.GetRandomCard(ElementType.None);
        else    
            cardName = CardManager.Instance.GetRandomCard(selectedType == ElementType.Ruin ? ElementType.Psychic : ElementType.Ruin);

        return CardManager.Instance.GetCardData(cardName);
    }
}