using UnityEngine;

public class TestUIController : MonoBehaviour
{
    [SerializeField] private GameObject addCardUI;

    public void EnableAddCardUI()
    {
        addCardUI.SetActive(true);
        BattleManager.Instance.Pause();
    }

    public void DisableAddCardUI()
    {
        addCardUI.SetActive(false);
        BattleManager.Instance.Restart();
    }
}