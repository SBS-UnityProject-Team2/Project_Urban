using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMain : MonoBehaviour
{
    public void OnClickMain()
    {
        SceneManager.LoadScene(SceneName.Main);
        SoundManager.Instance.PlayTitleSound();

        Destroy(PlayerManager.Instance.gameObject);
        Destroy(DeckManager.Instance.gameObject);
        Destroy(MapManager.Instance.gameObject);
    }
}
