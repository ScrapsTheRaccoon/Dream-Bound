using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip defaultLevelMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip gemCollect;
    [SerializeField] private AudioClip levelFinish;
    [SerializeField] private AudioClip fallOff;
    [SerializeField] private AudioClip timeUp;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip boing;

    [Header("Fade In & Fade Out")]
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float fadeOutDuration = 2f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepLoop;

    public float LevelFinishLength => levelFinish != null ? levelFinish.length : 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------- FADES ----------
    public void FadeOutMusic()
    {
        StartCoroutine(FadeOutRoutine(fadeOutDuration));
    }

    private IEnumerator FadeInRoutine(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            musicSource.volume = t;
            yield return null;
        }
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }

    // ---------- MUSIC ----------

    public void PlayMusic(AudioClip clip, bool fadeIn = false)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        if (musicSource.isPlaying)
            StopCoroutine(nameof(FadeOutRoutine));


        musicSource.clip = clip;
        musicSource.volume = fadeIn ? 0f : 1f;
        musicSource.Play();

        if (fadeIn)
            StartCoroutine(FadeInRoutine(fadeInDuration));
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ---------- SFX ----------

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void StartFootsteps()
    {
        if (footstepSource.isPlaying) return;

        footstepSource.clip = footstepLoop;
        footstepSource.Play();
    }

    public void StopFootsteps()
    {
        if (!footstepSource.isPlaying) return;

        footstepSource.Stop();
    }

    // ---------- CONVENIENCE METHODS ----------

    public void PlayGem()
    {
        PlaySFX(gemCollect);
    }

    public void PlayLevelFinish()
    {
        PlaySFX(levelFinish);
    }

    public void PlayFallOff()
    {
        PlaySFX(fallOff);
    }

    public void PlayTimeUp()
    {
        PlaySFX(timeUp);
    }

    public void PlayJump()
    {
        PlaySFX(jump);
    }

    public void PlayExplosion()
    {
        PlaySFX(explosion);
    }

    public void PlayBoing()
    {
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        PlaySFX(boing);
    }
}

