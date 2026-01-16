using UnityEngine;

public class AddCardList : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Transform startPos;
    [SerializeField] private float xSpacing;
    [SerializeField] private float ySpacing;

    [Header("Card Settings")]
    [SerializeField] private Transform content;
    [SerializeField] private UICard uiCardPrefab;

    private void Init()
    {
        
    }

    private UICard CreateUICard(CardDataEntry cardDataEntry, Vector3 position)
    {
        UICard uiCard = Instantiate(uiCardPrefab, position, Quaternion.identity, content);
    }
}