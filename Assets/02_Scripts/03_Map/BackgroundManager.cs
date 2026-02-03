using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BackgroundManager : SceneSingleton<BackgroundManager>
{
    [SerializeField] private Sprite mapBackground;
    [SerializeField] private Sprite restBackground;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetMapBg()
    {
        image.sprite = mapBackground;
    }

    public void SetRestBg()
    {
        image.sprite = restBackground;
    }
}
