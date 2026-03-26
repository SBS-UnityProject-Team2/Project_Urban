using TMPro;
using UnityEngine;


/* 
    기존 UICard 대체용 
    추가적으로 표기해야할 정보가 있다면 CardView를 상속받아 사용.
    ex) 상점용이면 상속받고 가격도 표시하도록 기능 추가
*/
[RequireComponent(typeof(SpriteRenderer))]
public class CardView : MonoBehaviour, ICardInstance
{
    [SerializeField] private TMP_Text textCardName;
    [SerializeField] private TMP_Text textCardDesc;
    [SerializeField] private TMP_Text textCost;

    private CardInstance cardInstance;

    virtual public void Init(CardInstance cardInstance)
    {
        this.cardInstance = cardInstance;

        CardDataEntry cardData = cardInstance.CardData;
        textCardName.text = $"{cardData.koreanName}";
        textCardDesc.text = $"{cardData.description}";
        textCost.text = $"{cardData.cost}";

        GetComponent<SpriteRenderer>().sprite = CardManager.Instance.GetCardImage(cardInstance.Name);
    }
}