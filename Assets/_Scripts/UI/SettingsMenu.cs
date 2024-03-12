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
    [Header("Submenu Canvas Groups")]
    [SerializeField] private CanvasGroup videoSubMenuCG = null;
    [SerializeField] private CanvasGroup audioSubMenuCG = null;

    [Header("Button References")]
    [SerializeField] private Button videoButton = null; // VIDEO SETTINGS NOT CURRENTLY IMPLEMENTED
    [SerializeField] private Button audioButton = null;
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

        videoSubMenuCG.alpha = 0f;
        audioSubMenuCG.alpha = 0f;

        audioButton.onClick.AddListener(() =>
        {
            OpenAudioSubMenu();
        } );

        returnButton.onClick.AddListener(() =>
        {
            // If audio submenu is open when return button is pressed, fade it out
            if (audioSubMenuOpen && audioSubMenuCG.alpha > 0f)
            {
                FadeOut(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear, () => { audioSubMenuOpen = false; });
            }

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

    public void OpenAudioSubMenu()
    {
        if (audioSubMenuOpen)
        {
            CloseAudioSubMenu();
            return;
        }
        audioSubMenuOpen = true;

        FadeIn(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear);
    }

    public void CloseAudioSubMenu()
    {
        if (audioSubMenuOpen)
        {
            FadeOut(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear);
 
            audioSubMenuOpen = false;
        }

        else
        {
            Debug.Log($"[SettingsMenu Error]: Audio SubMenu is not opened");
            return;
        }
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
        //AudioManager.instance.LoadMasterVolume();
    }

    private void OnSFXVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.SFX_VOLUME, argValue);
        //AudioManager.instance.LoadSFXVolume();
    }
    
    private void OnMusicVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.MUSIC_VOLUME, argValue);
        //AudioManager.instance.LoadMusicVolume();
    }

    private void OnDestroy()
    {
        audioButton.onClick.RemoveAllListeners();
        returnButton.onClick.RemoveAllListeners();
    }
}