using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MenuBase
{
    [Header("Button References")]
    [SerializeField] private UIButton startButton = null;
    [SerializeField] private UIButton settingsButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button tutorialButton = null;
    [SerializeField] private Button exitButton = null;

    [Header("Credits Panel")]
    [SerializeField] private CanvasGroup creditPanelCG = null;
    [SerializeField] private Button creditPanelBackButton = null;

    [Header("Tutorial Panel")]
    [SerializeField] private CanvasGroup tutorialPanelCG = null;
    [SerializeField] private Button tutorialPanelBackButton = null;



    public override void Init()
    {
        base.Init();

        // Start
        startButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.GameScene,
                                                                           LoadSceneMode.Single,
                                                                           () =>
                                                                           {
                                                                               AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.gameplayMusic);
                                                                           } ));
        } );

        // Settings
        settingsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.SettingsMenu,
                                                                           LoadSceneMode.Additive));
        } );

        // Credits
        creditsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeIn(creditPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                creditPanelCG.interactable = true;
                creditPanelCG.blocksRaycasts = true;
            } );
        } );

        creditPanelBackButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeOut(creditPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                creditPanelCG.interactable = false;
                creditPanelCG.blocksRaycasts = false;
            } );
        } );

        // Tutorial
        tutorialButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeIn(tutorialPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                tutorialPanelCG.interactable = true;
                tutorialPanelCG.blocksRaycasts = true;
            } );
        } );

        tutorialPanelBackButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeOut( tutorialPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                tutorialPanelCG.interactable = false;
                tutorialPanelCG.blocksRaycasts = false;
            } );
        } );

        // Exit
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        exitButton.gameObject.SetActive(true);
        exitButton.onClick.AddListener( () =>
        {
            Application.Quit();
        } );
#endif

        AudioManager.instance.LoadAudioSettings();
        AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.mainMenuMusic);
    }
}