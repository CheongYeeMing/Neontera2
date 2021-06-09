using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public NPC npc;

    public bool isTalking = false;

    public float distance;
    public float currResponseTracker = 0;

    public GameObject character;
    public GameObject dialogueWindow;

    public Text npcName;
    public Image npcFace;
    public Text npcDialogueBox;
    public Button[] characterResponses;

    public Animator animator;
    [SerializeField] DialogueFocus dialogueFocus;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsOpen", false);
    }

    public void Update()
    {
        if (!isTalking) return;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currResponseTracker++;
            if (currResponseTracker >= npc.characterDialogue.Length - 1)
            {
                currResponseTracker = npc.characterDialogue.Length - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currResponseTracker--;
            if (currResponseTracker < 0)
            {
                currResponseTracker = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            npcDialogueBox.text = npc.dialogue[(int)currResponseTracker+1];
        }
    }

    public void TriggerDialogue()
    {
        dialogueFocus.ToggleZoom();
        if (isTalking == false)
        {
            StartConversation();
        }
        else if (isTalking == true)
        {
            EndDialogue();
        }
    }

    public void StartConversation()
    {
        animator.SetBool("IsOpen", true);
        isTalking = true;
        currResponseTracker = 0;
        npcName.text = npc.npcName;
        npcFace.sprite = npc.icon;
        npcDialogueBox.text = npc.dialogue[0]; // Set to greeting message
        for (int i = 0; i < npc.characterDialogue.Length; i++)
        {
            characterResponses[i].gameObject.SetActive(true);
            characterResponses[i].GetComponentInChildren<Text>().text = npc.characterDialogue[i];  
        }
        // Clear selcted object
        EventSystem.current.SetSelectedGameObject(null);
        // Set new selected object
        EventSystem.current.SetSelectedGameObject(characterResponses[0].gameObject);
    }

    public void HideCharacterResponseOption()
    {
        for (int i = 0; i < npc.characterDialogue.Length; i++)
        {
            characterResponses[i].gameObject.SetActive(false);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        for (int i = 0; i < npc.characterDialogue.Length; i++)
        {
            characterResponses[i].gameObject.SetActive(false);
        }
        isTalking = false;
    }
}
