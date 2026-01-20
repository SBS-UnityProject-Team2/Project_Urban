using UnityEngine;

public class UIManager : SceneSingleton<UIManager>
{
    [SerializeField] private Canvas battleUI;
    [SerializeField] private Canvas battleEndUI;
    [SerializeField] private CanvasRenderer successUI;
    [SerializeField] private CanvasRenderer failUI;
    [SerializeField] private Canvas discardUI;

    private void Start()
    {
        battleUI.gameObject.SetActive(true);
        battleEndUI.gameObject.SetActive(false);

        BattleManager.Instance.OnBattleEnd.AddListener(HandleBattleEnd);
    }

    public void HandleBattleEnd(bool isVictory)
    {
        battleUI.gameObject.SetActive(false);
        battleEndUI.gameObject.SetActive(true);

        if (isVictory)
            successUI.gameObject.SetActive(true);
        else
            failUI.gameObject.SetActive(true);
    }

    public void OpenDiscardUI()
    {
        discardUI.gameObject.SetActive(true);
    }

    public void CloseDiscardUI()
    {
        discardUI.gameObject.SetActive(false);
    }
}