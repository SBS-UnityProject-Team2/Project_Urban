using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailUI : MonoBehaviour
{
    [Header("Text UI References")]
    [SerializeField] private TMP_Text clearedStage;
    [SerializeField] private TMP_Text enemiesKill;
    [SerializeField] private TMP_Text earnedCards;
    [SerializeField] private TMP_Text earnedStories;

    [Header("Text Label Settings")]
    [SerializeField] private string clearedStageLabel;
    [SerializeField] private string enemiesKillLabel;
    [SerializeField] private string earnedCardsLabel;
    [SerializeField] private string earnedStoriesLabel;

    private void Awake()
    {
        clearedStage.text = $"{clearedStageLabel} : 10";
        enemiesKill.text = $"{enemiesKillLabel} : 10";
        earnedCards.text = $"{earnedCardsLabel} : 10";
        earnedStories.text = $"{earnedStoriesLabel} : 10";
    }

    public void LoadMainScene()
    {
        BattleManager.Instance.Player.Hand.RemoveAll();
        GameManager.Instance.GameOver();
    }
}