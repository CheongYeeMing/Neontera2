using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] public GameObject AcceptedQuestWindow;
    [SerializeField] public SelectedQuestWindow selectedQuestWindow;

    public Animator animator;

    private bool isOpen = false;

    public void Start()
    {
        //AcceptedQuestWindow.SetActive(true);
        animator.SetBool("IsOpen", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuestList();
        }
    }

    public void ToggleQuestList()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
        selectedQuestWindow.gameObject.SetActive(false);
    }

    public void Close()
    {
        ToggleQuestList();
    }
}
