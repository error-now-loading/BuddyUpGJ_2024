using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuBase
{
    public static PauseMenu instance {  get; private set; }

    [SerializeField] private UIButton continueButton = null;
    [SerializeField] private UIButton restartButton = null;
    [SerializeField] private UIButton settingsButton = null;
    [SerializeField] private UIButton tutorialButton = null;
    [SerializeField] private UIButton returnToMainMenuButton = null;

    [Header("Tutorial Panel")]
    [SerializeField] private CanvasGroup tutorialPanelCG = null;
    [SerializeField] private UIButton tutorialPanelBackButton = null;

    private bool _isPaused = false;
    public bool isPaused => _isPaused;



    protected override void Start()
    {
        instance = this;

        // Continue
        continueButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            EventManager.instance.Notify(EventTypes.PauseMenuClosedExternally);
        } );

        // Restart
        restartButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.GameScene,
                                                                           LoadSceneMode.Single,
                                                                           () =>
                                                                           {
                                                                               Time.timeScale = 1f;
                                                                               AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.gameplayMusic);
                                                                           } ));
        });

        // Settings
        settingsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.SettingsMenu,
                                                                           LoadSceneMode.Additive));
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

            FadeOut(tutorialPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                tutorialPanelCG.interactable = false;
                tutorialPanelCG.blocksRaycasts = false;
            });
        } );

        // Return to Main Menu
        returnToMainMenuButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.MainMenu,
                                                                           LoadSceneMode.Single,
                                                                           () =>
                                                                           {
                                                                               Time.timeScale = 1f;
                                                                               AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.mainMenuMusic);
                                                                           } ));
        } );
    }

    public void PauseGame()
    {
        if (!_isPaused)
        {
            Time.timeScale = 0f;

            mainCG.alpha = 1f;
            mainCG.interactable = true;
            mainCG.blocksRaycasts = true;

            _isPaused = true;
        }

        else
        {
            Debug.Log("[PauseMenu Error]: PauseGame called but game is already paused");
        }
    }

    public void UnpauseGame()
    {
        if (_isPaused)
        {
            Time.timeScale = 1f;

            mainCG.alpha = 0f;
            mainCG.interactable = false;
            mainCG.blocksRaycasts = false;

            _isPaused = false;
        }
        
        else
        {
            Debug.Log("[PauseMenu Error]: UnpauseGame called but game is not paused");
        }
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.RemoveAllListeners();
        returnToMainMenuButton.onClick.RemoveAllListeners();
    }
}