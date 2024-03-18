using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     <para>
///         Menu controller for the Settings Menu
///     </para>
///     
///     SHOULD NOT BE INSTANTIATED VIA CODE, PREFAB FOR SETTINGS MENU SHOULD ONLY LIVE WITHIN ITS OWN SCENE
/// </summary>
public class SettingsMenu : MenuBase
{
    [Header("Button References")]
    [SerializeField] private Button returnButton = null;

    [Header("TextSlider References")]
    [SerializeField] private TextSlider masterVol = null;
    [SerializeField] private TextSlider sfxVol = null;
    [SerializeField] private TextSlider musicVol = null;

    private bool videoSubMenuOpen = false;
    private bool audioSubMenuOpen = false;



    public override void Init()
    {
        base.Init();

        returnButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeOut(mainCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.SettingsMenu) );
            } );
        } );

        // Audio submenu setup
        masterVol.AddListener(OnMasterVolChanged);
        sfxVol.AddListener(OnSFXVolChanged);
        musicVol.AddListener(OnMusicVolChanged);
        LoadAudioSliderSettings();
    }

    private void LoadAudioSliderSettings()
    {
        masterVol.SetSliderValue(SaveDataUtility.LoadFloat(SaveDataUtility.MASTER_VOLUME, 0.5f));
        sfxVol.SetSliderValue(SaveDataUtility.LoadFloat(SaveDataUtility.SFX_VOLUME, 0.5f));
        musicVol.SetSliderValue(SaveDataUtility.LoadFloat(SaveDataUtility.MUSIC_VOLUME, 0.5f));
    }

    private void OnMasterVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.MASTER_VOLUME, argValue);
        AudioManager.instance.LoadMasterVolume();
    }

    private void OnSFXVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.SFX_VOLUME, argValue);
        AudioManager.instance.LoadSFXVolume();
    }
    
    private void OnMusicVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.MUSIC_VOLUME, argValue);
        AudioManager.instance.LoadMusicVolume();
    }

    private void OnDestroy()
    {
        returnButton.onClick.RemoveAllListeners();
    }
}