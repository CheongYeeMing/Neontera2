using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        animator.SetTrigger("FadeIn");
    }

    public void Deactivate()
    {
        animator.SetTrigger("FadeOut");
    }
}
