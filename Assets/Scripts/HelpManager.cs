using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [SerializeField] public GameObject helpMenu;

    public Animator animator;

    private bool isOpen = false;

    // Start is called before the first frame update
    public void Start()
    {
        animator.SetBool("isOpen", false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHelpMenu();
        }
    }

    public void ToggleHelpMenu()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
}
