using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal Destination;
    [SerializeField] string Location;

    [SerializeField]  bool isActivated;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            animator.SetTrigger("Activate");
        }
        else
        {
            animator.SetTrigger("Deactivate");

        }
    }

    public void Teleport(GameObject Character)
    {
        if (isActivated)
        {
            StopAllCoroutines();
            StartCoroutine(Character.GetComponent<ParallaxBackgroundManager>().ChangeBackground(Destination.Location, Character, Destination));
        }
    }
}
