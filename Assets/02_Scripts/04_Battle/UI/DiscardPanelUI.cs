using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DiscardPanelUI : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private UICard uICardPrefab;

    [Header("UI Reference")]
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button confirmButton;

    [Header("Text Settings")]
    [SerializeField] private string titleString = "버릴 카드를 선택하세요";

    public UnityEvent<List<Card>> OnConfirm = new();

    private readonly Dictionary<Card, UICard> selectedCardMap = new();
    private int minCount;
    private int maxCount;

    private void Awake()
    {
        confirmButton.onClick.AddListener(() => OnConfirm?.Invoke(selectedCardMap.Keys.ToList()));
    }

    public void OpenPanel(int minCount, int maxCount)
    {
        Reset();

        this.minCount = minCount;
        this.maxCount = maxCount;
        
        gameObject.SetActive(true);
        UpdateUI();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void AddCard(Card card)
    {
        if (selectedCardMap.Count >= maxCount) return;

        UICard uICard = CreateUICard(card);

        selectedCardMap.Add(card, uICard);
        card.IsDiscardSelect = true;

        UpdateUI();
    }

    public void RemoveCard(Card card)
    {
        UICard uICard = selectedCardMap[card];

        selectedCardMap.Remove(card);
        card.IsDiscardSelect = false;

        UpdateUI();
        Destroy(uICard.gameObject);
    }

    private UICard CreateUICard(Card card)
    {
        UICard uICard = Instantiate(uICardPrefab, content);
        uICard.SetCardDataEntry(card.Data);

        Button button = uICard.gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => RemoveCard(card));

        return uICard;
    }

    private void UpdateUI()
    {
        int selectCount = selectedCardMap.Count;

        titleText.text = $"{titleString}({selectCount}/{maxCount})";
        confirmButton.enabled = selectCount >= minCount;
    }

    private void Reset()
    {
        selectedCardMap.Clear();
        OnConfirm.RemoveAllListeners();

        foreach (Transform child in content)
            Destroy(child.gameObject);
    }
}