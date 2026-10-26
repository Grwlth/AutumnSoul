
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip levelMusic;

    [Header("SFX")]
    public AudioClip switchCharacterSFX;
    public AudioClip attackSFX;
    public AudioClip ingredientSFX;
    public AudioClip leafSFX;
    public AudioClip jumpSFX;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Range(0f, 1f)]
    public float jumpVolume = 0.6f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupAudioSources();

        if (levelMusic != null)
        {
            PlayMusic();
        }
    }

    private void SetupAudioSources()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = sfxVolume;
        }
    }

    public void PlayMusic()
    {
        if (musicSource == null || levelMusic == null)
        {
            return;
        }

        musicSource.clip = levelMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlaySwitchCharacter()
    {
        PlaySFX(switchCharacterSFX);
    }

    public void PlayAttack()
    {
        PlaySFX(attackSFX);
    }

    public void PlayIngredient()
    {
        PlaySFX(ingredientSFX);
    }

    public void PlayLeaf()
    {
        PlaySFX(leafSFX);
    }

    public void PlayJump()
    {
        if (sfxSource == null || jumpSFX == null)
        {
            return;
        }

        sfxSource.PlayOneShot(
            jumpSFX,
            jumpVolume
        );
    }

    public void PlayWin()
    {
        PlaySFX(winSFX);
    }

    public void PlayLose()
    {
        PlaySFX(loseSFX);
    }
}
