using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UnusedCardList : MonoBehaviour
{

    [SerializeField] private UICard cardPrefab;
    [SerializeField] private Deck deck; 
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform panelContent;
    [SerializeField] private TMP_Text emptyMessageText;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void OnClick()
    {
        panel.SetActive(true);
        CardDisplay.Display(deck.UnusedCardList, panelContent, cardPrefab);

        bool hasCards = deck.UnusedCardList.Count > 0;
        emptyMessageText.gameObject.SetActive(!hasCards);
    }

    public void OnClose()
    {
        panel.SetActive(false);
        emptyMessageText.gameObject.SetActive(false);
    }
}