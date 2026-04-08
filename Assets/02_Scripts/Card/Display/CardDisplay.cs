using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private UICard uiCardPrefab;
    [SerializeField] private Button closeButton;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private RectTransform content;

    private void Awake()
    {
        closeButton.onClick.AddListener(Close);
    }

    public void Display(string title, List<DeckCard> cards)
    {
        titleText.text  = title;

        foreach (RectTransform child in content)
            Destroy(child.gameObject);

        foreach (DeckCard card in cards)
        {
            UICard uICard = Instantiate(uiCardPrefab, content);
            uICard.Init(card);
        }

        Battle.Instance.IsPause = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Battle.Instance.IsPause = false;
        gameObject.SetActive(false);
    }
}