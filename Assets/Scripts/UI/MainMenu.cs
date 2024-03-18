using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MenuBase
{
    [Header("Button References")]
    [SerializeField] private Button startButton = null;
    [SerializeField] private Button settingsButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button exitButton = null;

    [Header("Credits Panel")]
    [SerializeField] private CanvasGroup creditPanelCG = null;
    [SerializeField] private Button creditPanelBackButton = null;



    public override void Init()
    {
        base.Init();

        startButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.GameScene,
                                                                           LoadSceneMode.Single));
        } );

        settingsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)SceneIndices.SettingsMenu,
                                                                           LoadSceneMode.Additive));
        } );

        creditsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeIn(creditPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                creditPanelCG.interactable = true;
                creditPanelCG.blocksRaycasts = true;
            } );
        } );

        creditPanelBackButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonClick);

            FadeOut(creditPanelCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                creditPanelCG.interactable = false;
                creditPanelCG.blocksRaycasts = false;
            } );
        } );

        
    }

    private void ShowCreditsPanel()
    {

    }

    private void CloseCreditsPanel()
    {

    }
}