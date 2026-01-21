using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessUI : MonoBehaviour
{
    [SerializeField] private RewardUI rewardUI;
    [SerializeField] private CardSelectUI cardSelectUI;

    private void Awake()
    {
        ShowReward();
    }
    
    public void OnCardSelected(CardName cardName)
    {
        DeckManager.Instance.AddCard(cardName);
        rewardUI.DisableCardSelectButton();
        ShowReward();
    }

    public void OnTest()
    {
        Debug.Log("Click");
    }

    public void ShowReward()
    {
        rewardUI.gameObject.SetActive(true);
        cardSelectUI.gameObject.SetActive(false);
    }    

    public void ShowCardSelect()
    {
        cardSelectUI.Initialize(OnCardSelected);
        
        rewardUI.gameObject.SetActive(false);
        cardSelectUI.gameObject.SetActive(true);
    }

    public void LoadMapScene()
    {
        BattleManager.Instance.Player.Hand.RemoveAll();
        SceneManager.LoadScene(Scene.Map);
    }
}