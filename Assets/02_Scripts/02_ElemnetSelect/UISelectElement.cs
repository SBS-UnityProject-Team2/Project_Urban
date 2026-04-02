using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class UISelectElement : MonoBehaviour
{
    [SerializeField] private List<CardName> cards = new();

    public void SelectElement(ElementType element)
    {
        GameManager.Instance.SelectedElement = element;
        List<CardName> selectedCards = GetStarterCardsByElement(element);

        foreach (CardName cardName in selectedCards)
        {
            DeckManager.Instance.AddCard    (cardName);
        }

        SceneManager.LoadScene(SceneName.Map);
        BgmManager.Instance.PlayMapSound();
    }

    private List<CardName> GetStarterCardsByElement(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ruin:
                return GetCardsFromRange(0, 2);
            case ElementType.Psychic:
                return GetCardsFromRange(2, 2);
            case ElementType.Bio:
                return GetCardsFromRange(4, 2);
            default:
                return new List<CardName>();
        }
    }

    private List<CardName> GetCardsFromRange(int startIndex, int count)
    {
        List<CardName> result = new();
        int endIndex = Mathf.Min(startIndex + count, cards.Count);

        for (int i = startIndex; i < endIndex; i++)
            result.Add(cards[i]);

        return result;
    }

    public void OnClickFlame()
    {
        SelectElement(ElementType.Ruin);
    }

    // 2. Ice 속성 선택 버튼 연결
    public void OnClickIce()
    {
       SelectElement(ElementType.Psychic);
    }

    // 3. Grass 속성 선택 버튼 연결
    public void OnClickGrass()
    {
       SelectElement(ElementType.Bio);
    }
}