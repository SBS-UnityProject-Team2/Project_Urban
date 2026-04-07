using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private const string MasterVolumeKey = "MasterVolume";
    private const string BgmVolumeKey = "BgmVolume";

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float bgmVolume = 1f;

    [Header("Clip")]
    [SerializeField] private AudioClip titleSound;
    [SerializeField] private AudioClip mapSound;
    [SerializeField] private AudioClip shopSound;
    [SerializeField] private AudioClip restSound;
    [SerializeField] private AudioClip eliteSound;
    [SerializeField] private AudioClip bossSound;
    [SerializeField] private float fadeDuration = 1f;

    private AudioSource audioSource;
    private float defaultVolume;
    private float FinalBgmVolume => defaultVolume * masterVolume * bgmVolume;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        defaultVolume = audioSource.volume;

        LoadVolumes();
    }

    private void Start()
    {
        audioSource.clip = titleSound;
        ApplyFinalBgmVolume();
        audioSource.Play();
    }

    private void LoadVolumes()
    {
        masterVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MasterVolumeKey, masterVolume));
        bgmVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(BgmVolumeKey, bgmVolume));
    }

    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
        PlayerPrefs.SetFloat(BgmVolumeKey, bgmVolume);
        PlayerPrefs.Save();
    }

    private void ApplyFinalBgmVolume()
    {
        audioSource.volume = FinalBgmVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        SaveVolumes();
        ApplyFinalBgmVolume();
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        SaveVolumes();
        ApplyFinalBgmVolume();
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetBgmVolume()
    {
        return bgmVolume;
    }

    public void PlayTitleSound()
    {
        PlayClip(titleSound);
    }

    public void PlayMapSound()
    {
        PlayClip(mapSound);
    }

    public void PlayShopSound()
    {
        PlayClip(shopSound);
    }

    public void PlayRestSound()
    {
        PlayClip(restSound);
    }

    public void PlayEliteSound()
    {
        PlayClip(eliteSound);
    }

    public void PlayBossSound()
    {
        PlayClip(bossSound);
    }

    private void PlayClip(AudioClip clip)
    {
        StartCoroutine(FadeOutIn(clip));
    }

    private IEnumerator FadeOutIn(AudioClip newClip)
    {
        float startVolume = audioSource.volume;
        float targetVolume = FinalBgmVolume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}