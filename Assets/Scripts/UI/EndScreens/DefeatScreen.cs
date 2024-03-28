using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScreen : MenuBase
{
    [SerializeField] private UIButton restartButton = null;
    [SerializeField] private UIButton mainMenuButton = null;



    protected override void Start()
    {
        EventManager.instance.Subscribe(EventTypes.PlayerDeath, Init);

        restartButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.GameScene,
                                                                           LoadSceneMode.Single,
                                                                           () =>
                                                                           {
                                                                               Time.timeScale = 1f;
                                                                               AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.gameplayMusic);
                                                                           } ));
        } );

        mainMenuButton.onClick.AddListener( () =>
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

    public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log($"[{gameObject.name} Error]: Already initialized");
            return;
        }
        isInitialized = true;

        mainCG.interactable = true;
        mainCG.blocksRaycasts = true;

        AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.defeatMusic);

        FadeIn(mainCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
        {
            PauseMenu.instance.SetGameEnd();
        } );
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        EventManager.instance.Unsubscribe(EventTypes.PlayerDeath, Init);
    }
}