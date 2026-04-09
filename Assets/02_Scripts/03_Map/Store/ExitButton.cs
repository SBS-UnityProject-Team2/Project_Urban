using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
	public void OnClickBackToMap()
	{
		SoundManager.Instance.PlayMapSound();
		SceneManager.LoadScene(SceneName.Map);
	}
}
