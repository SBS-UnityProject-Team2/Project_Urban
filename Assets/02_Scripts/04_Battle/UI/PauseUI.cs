using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ModalWindowManager))]
public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneName.Main);
            SoundManager.Instance.PlayTitleSound();

            Destroy(PlayerManager.Instance.gameObject);
            Destroy(DeckManager.Instance.gameObject);
            Destroy(MapManager.Instance.gameObject);
        });

        closeButton.onClick.AddListener(() =>
        {
            Battle.Instance.IsPause = false;
            GetComponent<ModalWindowManager>().ModalWindowOut();
        });
    }

    public void Open()
    {
        Battle.Instance.IsPause = true;
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }
}