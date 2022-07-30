using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator buttonAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
        HoverOff();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverOn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverOff();
    }

    public void HoverOn()
    {
        buttonAnimator.Play("HoverOn");
    }

    public void HoverOff()
    {
        try
        {
            buttonAnimator.Play("HoverOff");
        }
        catch (System.NullReferenceException ex)
        {
        }
    }
}
