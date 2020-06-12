using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSoundEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    //Called when the mouse button is pressed on the button.
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.MENU_CLICK);
    }

    //Called when the mouse cursor enters the button.
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.MENU_HOVER);
    }

}
