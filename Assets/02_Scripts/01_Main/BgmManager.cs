using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : Singleton<BgmManager>
{
    [SerializeField] private AudioClip titleSound;
    [SerializeField] private AudioClip mapSound;
    [SerializeField] private AudioClip shopSound;
    [SerializeField] private AudioClip restSound;
    [SerializeField] private AudioClip eliteSound;
    [SerializeField] private AudioClip bossSound;
    [SerializeField] private float fadeDuration = 1f;

    private AudioSource audioSource;
    private float defaultVolume;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        defaultVolume = audioSource.volume;
        
        PlayTitleSound();
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
            audioSource.volume = Mathf.Lerp(0f, defaultVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = defaultVolume;
    }
}