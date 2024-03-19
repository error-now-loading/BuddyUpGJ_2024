using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Mixer References")]
    [SerializeField] private AudioMixer master = null;
    private const string MASTER_VOL = "masterVol";
    private const string MUSIC_VOL = "musicVol";
    private const string SFX_VOL = "sfxVol";

    [Header("Global AudioSource References")]
    [SerializeField] public AudioSource sourceMaster = null;
    [SerializeField] public AudioSource sourceMusic = null;
    [SerializeField] public AudioSource sourceSFX = null;

    [Header("Music")]
    [SerializeField] public AudioClip mainMenuMusic = null;
    [SerializeField] public AudioClip gameplayMusic = null;
    [SerializeField] public AudioClip defeatMusic = null;
    [SerializeField] public AudioClip victoryMusic = null;

    [Header("UI SFX")]
    [SerializeField] public SFXVariants uiButtonHoverVariants = null;
    [SerializeField] public AudioClip uiButtonClick = null;

    [Header("Player SFX")]
    [SerializeField] public SFXVariants playerFootsteps = null;
    [SerializeField] public SFXVariants playerSummons = null;
    [SerializeField] public SFXVariants playerSummonFails = null;
    [SerializeField] public AudioClip playerCommand = null;
    [SerializeField] public AudioClip playerDeath = null;

    [Header("Minion SFX")]
    [SerializeField] public AudioClip minionAttack = null;
    [SerializeField] public AudioClip minionGet = null;
    [SerializeField] public AudioClip minionDeath = null;

    [Header("Bug SFX")]
    [SerializeField] public AudioClip bugAttack = null;
    [SerializeField] public AudioClip bugDeath = null;

    [Header("Slug SFX")]
    [SerializeField] public AudioClip slugAttack = null;
    [SerializeField] public AudioClip slugDeath = null;
    [SerializeField] public AudioClip slugMovement = null;



    private void Start()
    {
        LoadAudioSettings();
    }

    public void PlaySFX(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip;
        _source.volume = 1.0f;
        _source.Play();
    }

    public void PlaySFXLoop(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip;
        _source.volume = 1.0f;
        _source.loop = true;
        _source.Play();
    }

    public void StopSFXLoop(AudioSource _source)
    {
        _source.loop = false;
        _source.Stop();
    }

    public void PlayRandomPitchSFX(AudioSource _source, AudioClip audioClip, float low = 0.75f, float high = 1.25f)
    {
        // Only for the dialogue clicks 
        _source.clip = audioClip;
        _source.pitch = Random.Range(0.7f, 1.25f);
        _source.Play();
    }

    public void StartAmbienceLoop(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip;
        _source.loop = true;
        _source.volume = 0.4f;
        _source.Play();
    }

    public void PlayMusic(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip;
        _source.loop = true;
        _source.volume = 0.6f;
        _source.Play();
    }

    private void LoadAudioSettings()
    {
        LoadMasterVolume();
        LoadMusicVolume();
        LoadSFXVolume();
    }

    public void LoadMasterVolume()
    {
        master.SetFloat(MASTER_VOL, Mathf.Log(SaveDataUtility.LoadFloat(SaveDataUtility.MASTER_VOLUME)) * 20);
    }

    public void LoadMusicVolume()
    {
        master.SetFloat(MUSIC_VOL, Mathf.Log(SaveDataUtility.LoadFloat(SaveDataUtility.MUSIC_VOLUME)) * 20);
    }

    public void LoadSFXVolume()
    {
        master.SetFloat(SFX_VOL, Mathf.Log(SaveDataUtility.LoadFloat(SaveDataUtility.SFX_VOLUME)) * 20);
    }
}