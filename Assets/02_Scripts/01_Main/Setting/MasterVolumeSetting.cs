using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSetting : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private bool isReady;
    private bool isSyncing;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnEnable()
    {
        if (isReady)
        {
            SyncSliderValue();
        }
    }

    private void Start()
    {
        SyncSliderValue();
        isReady = true;
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float value)
    {
        if (!isReady || isSyncing)
        {
            return;
        }

        SoundManager.Instance.SetMasterVolume(NormalizeSliderValue(value));
    }

    private void SyncSliderValue()
    {
        isSyncing = true;
        slider.value = DenormalizeSliderValue(SoundManager.Instance.GetMasterVolume());
        isSyncing = false;
    }

    private float NormalizeSliderValue(float value)
    {
        return Mathf.InverseLerp(slider.minValue, slider.maxValue, value);
    }

    private float DenormalizeSliderValue(float value)
    {
        return Mathf.Lerp(slider.minValue, slider.maxValue, value);
    }
}
