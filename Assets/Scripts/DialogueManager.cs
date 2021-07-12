using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public NPC npc;
    [SerializeField] GameObject NpcNameTag;

    public bool isTalking = false;

    public float distance;
    public float currResponseTracker = 0;

    public GameObject character;
    public GameObject dialogueWindow;
    [SerializeField] TextMeshProUGUI enterToContinue;

    public Text npcName;
    public Image npcFace;
    public Text npcDialogueBox;
    public Button[] characterResponses;

    public Animator animator;
    [SerializeField] DialogueFocus dialogueFocus;

    [SerializeField] QuestList questList;

    public Quest quest;
    [SerializeField] public SelectedQuestWindow selectedQuestWindow;
    [SerializeField] public ShopSelectedItemPanel shopSelectedItemPanel;

    [SerializeField] public TransitionManager transition;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsOpen", false);
    }

    public void Update()
    {
        NpcNameTag.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1);
        if (!isTalking) return;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currResponseTracker++;
            if (currResponseTracker >= npc.Sequences[npc.sequenceNumber].characterDialogue.Length - 1)
            {
                currResponseTracker = npc.Sequences[npc.sequenceNumber].characterDialogue.Length - 1;
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
            FindObjectOfType<AudioManager>().StopEffect("RetroClick");
            FindObjectOfType<AudioManager>().PlayEffect("RetroClick");
            if (npc.Sequences[npc.sequenceNumber].isStory)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length - 1)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    currResponseTracker++;
                }
                else
                {
                    if (npc.Sequences[npc.sequenceNumber].teleport)
                    {
                        StopAllCoroutines();
                        StartCoroutine(FindObjectOfType<ParallaxBackgroundManager>().Teleport(npc.Sequences[npc.sequenceNumber].newBG, character, npc.Sequences[npc.sequenceNumber].characterV2));
                    }
                    else
                    {
                        npc.sequenceNumber++;
                        StartConversation();
                    }
                }
            }
            else if (npc.Sequences[npc.sequenceNumber].hasShop)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    HideCharacterResponseOption();
                    OpenShop();
                }
                else
                {
                    TriggerDialogue();
                }
            }
            else if (npc.Sequences[npc.sequenceNumber].hasQuest)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    HideCharacterResponseOption();
                    OpenQuestWindow();
                }
                else
                {
                    TriggerDialogue();
                }
            }
            else if (npc.Sequences[npc.sequenceNumber].waitingQuest)
            {
                TriggerDialogue();
            }
            else if (npc.Sequences[npc.sequenceNumber].justDialogue)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    HideCharacterResponseOption();
                    currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length;
                }
                else
                {
                    npc.sequenceNumber++;
                    StartConversation();
                }
            }
            else if (npc.Sequences[npc.sequenceNumber].hasHeal)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length - 1)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    HideCharacterResponseOption();
                    HealCharacter();
                    currResponseTracker++;
                }
                else
                {
                    TriggerDialogue();
                }
            }
            else if (npc.Sequences[npc.sequenceNumber].isEnd)
            {
                if (npc.Sequences[npc.sequenceNumber].repeatableQuest)
                {
                    if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                        HideCharacterResponseOption();
                        OpenQuestWindow();
                    }
                    else
                    {
                        TriggerDialogue();
                    }
                }
                else // Just story
                {

                }
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
        FindObjectOfType<Character>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        FindObjectOfType<AudioManager>().StopEffect("Run");
        FindObjectOfType<AudioManager>().StopEffect("DialogueMonologue");
        FindObjectOfType<AudioManager>().PlayEffect("DialogueMonologue");
        currResponseTracker = 0;
        npcName.text = npc.npcName;
        npcFace.sprite = npc.icon;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[0]));
        for (int i = 0; i < npc.Sequences[npc.sequenceNumber].characterDialogue.Length; i++)
        {
            characterResponses[i].gameObject.SetActive(true);
            characterResponses[i].GetComponentInChildren<Text>().text = npc.Sequences[npc.sequenceNumber].characterDialogue[i];  
        }
        // Clear selcted object
        EventSystem.current.SetSelectedGameObject(null);
        // Set new selected object
        EventSystem.current.SetSelectedGameObject(characterResponses[0].gameObject);

        foreach (Quest quest in questList.quests)
        {
            if (quest.questCriteria.criteriaType == CriteriaType.Talk)
            {
                Debug.Log(quest.questCriteria.Target);
                Debug.Log(npc.npcName);
                if (quest.questCriteria.Target == npc.npcName)
                {
                    quest.questCriteria.Execute();
                    quest.Update();
                }
            }
        }
    }

    public void HideCharacterResponseOption()
    {
        for (int i = 0; i < npc.Sequences[npc.sequenceNumber].characterDialogue.Length; i++)
        {
            characterResponses[i].gameObject.SetActive(false);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        for (int i = 0; i < npc.Sequences[npc.sequenceNumber].characterDialogue.Length; i++)
        {
            characterResponses[i].gameObject.SetActive(false);
        }
        isTalking = false;
        CloseShop();
        CloseQuestWindow();
    }

    public IEnumerator TypeSentence(string sentence)
    {
        enterToContinue.gameObject.SetActive(false);
        npcDialogueBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            npcDialogueBox.text += letter;
            yield return null;
        }
        enterToContinue.gameObject.SetActive(true);
    }

    public void OpenShop()
    {
        if (!isTalking) return;
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        ShopManager shopManager;
        if (currResponseTracker == 0 && gameObject.TryGetComponent<ShopManager>(out shopManager) == true && npc.Sequences[npc.sequenceNumber].hasShop)
        {
            shopManager.ShopWindow.GetComponentInChildren<Shop>().SetItems(npc.Sequences[npc.sequenceNumber].Items);
            shopManager.ShopWindow.gameObject.SetActive(true);
        }
    }

    public void CloseShop()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        ShopManager shopManager;
        if (gameObject.TryGetComponent<ShopManager>(out shopManager) == true)
        {
            shopManager.shopSelectedItemPanel.gameObject.SetActive(false);
            shopManager.ShopWindow.gameObject.SetActive(false);
        }
        currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker - 1]));
    }

    public void OpenQuestWindow()
    {
        if (!isTalking) return;
        quest = npc.Sequences[npc.sequenceNumber].Quest;
        quest.npc = npc;
        quest.Reset();
        selectedQuestWindow.gameObject.SetActive(true);
        selectedQuestWindow.dialogueManager = this;
        selectedQuestWindow.QuestSelected(quest);
    }

    public void CloseQuestWindow()
    {
        selectedQuestWindow.gameObject.SetActive(false);
    }

    public void QuestAccepted()
    {
        currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length - 2;
        Debug.Log(npc.Sequences[npc.sequenceNumber].dialogue.Length);
        Debug.Log(npc.sequenceNumber);
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker]));
        npc.sequenceNumber++;
    }

    public void QuestDeclined()
    {
        currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length - 1;
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker]));
    }

    public void HealCharacter()
    {
        if (currResponseTracker != 0) return;
        FindObjectOfType<CharacterHealth>().FullRestore();
    }
}
