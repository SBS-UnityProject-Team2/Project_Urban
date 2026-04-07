using UnityEngine;

public class SettingButton : MonoBehaviour
{
	[SerializeField] private GameObject settingPanel;

	public void OnClickOpenSetting()
	{
		settingPanel.SetActive(true);
	}

	public void OnClickCloseSetting()
	{
		settingPanel.SetActive(false);
	}
}
