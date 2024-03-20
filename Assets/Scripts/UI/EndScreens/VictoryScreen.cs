using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MenuBase
{
    [SerializeField] private CanvasGroup buttonCG = null;
    [SerializeField] private UIButton mainMenuButton = null;
    [SerializeField] private Animator victoryAnim = null;



    protected override void Start()
    {
        EventManager.instance.Subscribe(EventTypes.Victory, Init);

        mainMenuButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            PersistentSceneManager.instance.UnloadSceneAsync( (int)SceneIndices.GameScene, () =>
            {
                PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.MainMenu,
                                                                LoadSceneMode.Single,
                                                                () =>
                                                                {
                                                                    Time.timeScale = 1f;
                                                                    AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.gameplayMusic);
                                                                } );
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

        FadeIn(mainCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
        {
            victoryAnim.gameObject.SetActive(true);
            StartCoroutine(FadeInButton());
        } );
    }

    private IEnumerator FadeInButton()
    {
        // Gross hardcoded value in the interest of time, represents animation length in seconds * speed modifier
        yield return new WaitForSeconds(2.566f + fadeDuration + fadeStartDelay);

        FadeIn(buttonCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
        {
            buttonCG.interactable = true;
            buttonCG.blocksRaycasts = true;

            Time.timeScale = 0f;
        } );
    }

    private void OnDestroy()
    {
        mainMenuButton.onClick.RemoveAllListeners();
        EventManager.instance.Unsubscribe(EventTypes.Victory, Init);
    }
}