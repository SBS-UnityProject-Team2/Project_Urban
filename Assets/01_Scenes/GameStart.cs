using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene(Scene.ElementSelect);
    }
    
    public void OnQuitGame()
    {        
        Application.Quit();
    }

    public void OnTestSceneLoad()
    {
        SceneManager.LoadScene("05_TestScene");
    }
}
