using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Text characterResponse;

    public Animator animator;
    [SerializeField] DialogueFocus dialogueFocus;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsOpen", false);
    }

    public void Update()
    {
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
        if (currResponseTracker == 0 && npc.characterDialogue.Length >= 0)
        {
            characterResponse.text = npc.characterDialogue[0];
            if (Input.GetKeyDown(KeyCode.Return))
            {
                npcDialogueBox.text = npc.dialogue[1];
            }
        }
        else if (currResponseTracker == 1 && npc.characterDialogue.Length >= 1)
        {
            characterResponse.text = npc.characterDialogue[1];
            if (Input.GetKeyDown(KeyCode.Return))
            {
                npcDialogueBox.text = npc.dialogue[2];
            }
        }
        else if (currResponseTracker == 2 && npc.characterDialogue.Length >= 2)
        {
            characterResponse.text = npc.characterDialogue[2];
            if (Input.GetKeyDown(KeyCode.Return))
            {
                npcDialogueBox.text = npc.dialogue[3];
            }
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
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        isTalking = false;
    }
}
