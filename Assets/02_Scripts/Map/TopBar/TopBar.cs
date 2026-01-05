using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    [SerializeField] private HealthView healthView;
    [SerializeField] private CoinView coinView;
    [SerializeField] private Button toMainButton;

    private void Awake()
    {
        healthView.Bind(GameManager.Instance.PlayerHealth);
        coinView.Bind(GameManager.Instance.Coin);
        toMainButton.onClick.AddListener(() => GameManager.Instance.GameOver());
    }
}
