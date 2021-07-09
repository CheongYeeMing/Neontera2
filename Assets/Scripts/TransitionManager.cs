using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    private void OnValidate()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
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

    public void Inactive()
    {
        gameObject.SetActive(false);
    }
}
