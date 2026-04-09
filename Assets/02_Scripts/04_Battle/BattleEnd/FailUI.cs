using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneName.Main);
            SoundManager.Instance.PlayTitleSound();
        });
    }
}