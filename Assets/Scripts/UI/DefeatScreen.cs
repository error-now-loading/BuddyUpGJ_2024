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

            PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.MainMenu );
        } );

        mainMenuButton.onClick.AddListener( () =>
        {
            PersistentSceneManager.instance.LoadSceneAsync ( (int)SceneIndices.GameScene,
                                                             LoadSceneMode.Single,
                                                             () =>
                                                             {
                                                                 AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.gameplayMusic);
                                                             } );
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

        FadeIn(mainCG, fadeDuration, fadeStartDelay, EaseType.linear);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        if(EventManager.instance != null)
        {
            EventManager.instance.Unsubscribe(EventTypes.PlayerDeath, Init);
        }
    }
}