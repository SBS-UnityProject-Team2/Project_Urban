using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Button selectCardButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private RectTransform selectCardUI;

    private void Awake()
    {
        exitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayMapSound();
            SceneManager.LoadScene(SceneName.Map);
        });

        selectCardButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            selectCardUI.gameObject.SetActive(true);
        });
    }

    private void OnEnable()
    {
        coinText.text = $"{Battle.Instance.EarnCoin} Coin";
    }
}