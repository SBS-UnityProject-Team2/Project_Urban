using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddCardList : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private RectTransform startPos;
    [SerializeField] private float xSpacing;
    [SerializeField] private float ySpacing;

    [Header("Card Settings")]
    [SerializeField] private RectTransform content;
    [SerializeField] private UICard uiCardPrefab;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        float startXPos = startPos.localPosition.x;
        float startYPos = startPos.localPosition.y;
        int xIdx = 0;
        int yIdx = 0;

        List<CardDataEntry> cardDataEntries = CardManager.Instance.GetAllCardData().ToList();

        foreach (CardDataEntry cardDataEntry in cardDataEntries)
        {
            Vector3 position = new(startXPos + xIdx * xSpacing, startYPos - yIdx * ySpacing, 0);
            CreateUICard(cardDataEntry, position);

            xIdx++;
            
            if (xIdx % 5 == 0)
            {
                xIdx = 0;
                yIdx++;
            }
        }
        
    }

    private UICard CreateUICard(CardDataEntry cardDataEntry, Vector3 position)
    {
        UICard uiCard = Instantiate(uiCardPrefab, content);
        uiCard.GetComponent<RectTransform>().localPosition = position;
        uiCard.SetCardDataEntry(cardDataEntry);

        return uiCard;
    }
}