using UnityEngine;

public class BattleEndUI : MonoBehaviour
{
    [SerializeField] private RectTransform successUI;
    [SerializeField] private RectTransform failUI;
    
    private void Start()
    {
        gameObject.SetActive(false);

        Battle.Instance.OnBattleEnd.AddListener(isPlayerWin =>
        {
            gameObject.SetActive(true);

            if (isPlayerWin) successUI.gameObject.SetActive(true);
            else failUI.gameObject.SetActive(true);
        });
    }
}