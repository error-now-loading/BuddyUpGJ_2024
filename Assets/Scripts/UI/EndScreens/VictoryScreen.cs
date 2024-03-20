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
            PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.GameScene,
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

        FadeIn(mainCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
        {
            victoryAnim.gameObject.SetActive(true);
        } );
    }

    private IEnumerator FadeInButton()
    {
        yield return new WaitForSeconds(victoryAnim.GetCurrentAnimatorClipInfo(0).Length);

        FadeIn(buttonCG, 0.4f, fadeStartDelay, EaseType.linear, () =>
        {
            buttonCG.interactable = true;
            buttonCG.blocksRaycasts = true;
        } );
    }

    private void OnDestroy()
    {
        mainMenuButton.onClick.RemoveAllListeners();
        EventManager.instance.Unsubscribe(EventTypes.Victory, Init);
    }
}