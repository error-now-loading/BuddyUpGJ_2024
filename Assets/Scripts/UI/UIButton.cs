using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : Button, IPointerEnterHandler
{
    public override void OnPointerEnter(PointerEventData data)
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonHover1);
        }
        
        else
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sourceSFX, AudioManager.instance.uiButtonHover2);
        }

        base.OnPointerEnter(data);
    }
}