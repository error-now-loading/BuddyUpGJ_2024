using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite originalSprite;
    [SerializeField] private Sprite switchToSprite;

    private Image imageComponent;

    private void OnEnable()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.sprite = switchToSprite;
    }

    private void OnDisable()
    {
        imageComponent.sprite = originalSprite;
    }
}
