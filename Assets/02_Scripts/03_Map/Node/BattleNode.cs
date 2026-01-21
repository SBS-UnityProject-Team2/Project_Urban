using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BattleNode : MonoBehaviour
{
    private void Awake()
    {
        Button button =  GetComponent<Button>();
        button.onClick.AddListener(() => SceneManager.LoadScene(Scene.Battle));
    }
}