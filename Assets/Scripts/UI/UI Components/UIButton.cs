using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : Button, IPointerEnterHandler
{
    public override void OnPointerEnter(PointerEventData data)
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonHoverVariants.SelectRandom());

        base.OnPointerEnter(data);
    }
}