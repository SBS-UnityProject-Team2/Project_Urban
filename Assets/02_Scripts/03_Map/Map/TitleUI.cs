using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	public void OnClickGameStart()
	{
		SceneManager.LoadScene(SceneName.ElementSelect);
	}
}
