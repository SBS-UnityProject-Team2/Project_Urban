using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private GameObject settingsPrefab;

	public void OnClickGameStart()
	{
		SceneManager.LoadScene(SceneName.ElementSelect);
	}

	public void OnClickSettings()
	{
		settingsPrefab.SetActive(true);
	}

	public void OnClickSettingsExit()
	{
		settingsPrefab.SetActive(false);
	}
}
