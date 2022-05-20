using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private const float TEXT_TYPING_SPEED = 0.008f;
    private const int FIRST_RESPONSE = 0;
    private const int SECOND_RESPONSE = 1;
    private const string ANIMATOR_IS_OPEN = "IsOpen";
    private const string AUDIO_CLICK = "Click";
    private const string AUDIO_DIALOGUE_MONOLOGUE = "DialogueMonologue";
    private const string AUDIO_OPEN = "Open";
    private const string AUDIO_RETRO_CLICK = "RetroClick";
    private const string AUDIO_RUN = "Run";

    [SerializeField] private Animator animator;
    [SerializeField] private Button Close;
    [SerializeField] private Button[] characterResponses;
    [SerializeField] private DialogueFocus dialogueFocus;
    [SerializeField] private GameObject boss;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject npcNameTag;
    [SerializeField] private NPC npc;
    [SerializeField] private Text npcName;
    [SerializeField] private Image npcFace;
    [SerializeField] private Text npcDialogueBox;
    [SerializeField] private QuestList questList;
    [SerializeField] private SelectedQuestWindow selectedQuestWindow;
    [SerializeField] private ShopSelectedItemPanel shopSelectedItemPanel;
    [SerializeField] private TextMeshProUGUI enterToContinue;
    [SerializeField] private TransitionManager transition;

    public GameObject character;
    public GameObject dialogueWindow;
    private Quest quest;    

    public bool isTalking = false;
    public float distance;
    public float currResponseTracker = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsOpen", false);
        Close.onClick.AddListener(CloseShop);
    }

    public void Update()
    {
        npcNameTag.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        if (!isTalking)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down");
            currResponseTracker++;
            if (currResponseTracker >= npc.Sequences[npc.sequenceNumber].characterDialogue.Length - 1)
            {
                currResponseTracker = npc.Sequences[npc.sequenceNumber].characterDialogue.Length - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up");
            currResponseTracker--;
            if (currResponseTracker < 0)
            {
                currResponseTracker = 0;
            }
        }
        
        // Next
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<AudioManager>().StopEffect(AUDIO_RETRO_CLICK);
            FindObjectOfType<AudioManager>().PlayEffect(AUDIO_RETRO_CLICK);
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
                    if (npc.Sequences[npc.sequenceNumber].activatePortal)
                    {
                        foreach(Portal portal in FindObjectsOfType<Portal>())
                        {
                            if (portal.GetDestinationPortal().GetPortalLocation() == npc.Sequences[npc.sequenceNumber].destination)
                            {
                                portal.Activate();
                            }
                        }
                    }

                    // Activates Boss
                    if (npc.Sequences[npc.sequenceNumber].triggerBoss)
                    {
                        boss.gameObject.SetActive(true);
                    }

                    // Remove Quest Item from Inventory
                    if (npc.Sequences[npc.sequenceNumber].removeItem)
                    {
                        inventory.RemoveItem(npc.Sequences[npc.sequenceNumber].item);
                    }

                    // Add Item to Inventory
                    if (npc.Sequences[npc.sequenceNumber].addItem)
                    {
                        inventory.AddItem(npc.Sequences[npc.sequenceNumber].giveItem);
                    }

                    // Teleports Character
                    if (npc.Sequences[npc.sequenceNumber].teleport)
                    {
                        TriggerDialogue();
                        StopAllCoroutines();
                        StartCoroutine(FindObjectOfType<ParallaxBackgroundManager>().Teleport(npc.Sequences[npc.sequenceNumber].newBG, character, npc.Sequences[npc.sequenceNumber].characterV2));
                    }
                    // Reset Redoable Quest
                    else if (npc.Sequences[npc.sequenceNumber].backToNormal)
                    {
                        npc.sequenceNumber = npc.Sequences[npc.sequenceNumber].sequenceNum;
                        TriggerDialogue();
                    }
                    // Increment to next Sequence
                    else
                    {
                        npc.sequenceNumber++;
                        StartConversation();
                    }
                }
            }

            // Shop
            else if (npc.Sequences[npc.sequenceNumber].hasShop)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    HideCharacterResponseOption();
                    if (!OpenShop()) currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length;
                }
                else
                {
                    TriggerDialogue();
                }
            }

            // Quest
            else if (npc.Sequences[npc.sequenceNumber].hasQuest)
            {
                if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                    HideCharacterResponseOption();
                    if (!OpenQuestWindow()) currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length;
                }
                else
                {
                    TriggerDialogue();
                }
            }

            // Ongoing Quest
            else if (npc.Sequences[npc.sequenceNumber].waitingQuest)
            {
                TriggerDialogue();
            }

            // Just talk
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

            // Jeanne Heal
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

            // End of Sequence
            else if (npc.Sequences[npc.sequenceNumber].isEnd)
            {
                // Repeatable Quest
                if (npc.Sequences[npc.sequenceNumber].repeatableQuest)
                {
                    if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                        HideCharacterResponseOption();
                        if (!OpenQuestWindow()) currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length;
                    }
                    else
                    {
                        TriggerDialogue();
                    }
                }

                // Just Story
                else
                {
                    if (currResponseTracker < npc.Sequences[npc.sequenceNumber].dialogue.Length - 1)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker + 1]));
                        currResponseTracker++;
                    }
                    else
                    {
                        TriggerDialogue();
                    }
                }
            }
        }
    }

    public void TriggerDialogue()
    {
        dialogueFocus.ToggleZoom();
        if (!isTalking)
        {
            StartConversation();
        }
        else if (isTalking)
        {
            EndDialogue();
        }
    }

    private void StartConversation()
    {
        animator.SetBool(ANIMATOR_IS_OPEN, true);
        isTalking = true;
        FindObjectOfType<Character>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_RUN);
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_DIALOGUE_MONOLOGUE);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_DIALOGUE_MONOLOGUE);
        currResponseTracker = FIRST_RESPONSE;
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

        foreach (Quest quest in questList.questList)
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
        animator.SetBool(ANIMATOR_IS_OPEN, false);
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
            yield return new WaitForSeconds(TEXT_TYPING_SPEED);//null;
        }
        enterToContinue.gameObject.SetActive(true);
    }

    public bool OpenShop()
    {
        if (!isTalking)
        {
            return false;
        }
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_OPEN);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_OPEN);
        ShopManager shopManager;
        if (currResponseTracker == FIRST_RESPONSE && gameObject.TryGetComponent(out shopManager) == true 
            && npc.Sequences[npc.sequenceNumber].hasShop)
        {
            shopManager.ShopWindow.GetComponentInChildren<Shop>().SetItems(npc.Sequences[npc.sequenceNumber].Items);
            shopManager.ShopWindow.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    public void CloseShop()
    {
        if (!gameObject.activeSelf || !isTalking)
        {
            return;
        }
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_OPEN);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_OPEN);
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_CLICK);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_CLICK);
        ShopManager shopManager;
        if (gameObject.TryGetComponent(out shopManager) == true)
        {
            shopManager.shopSelectedItemPanel.gameObject.SetActive(false);
            shopManager.ShopWindow.gameObject.SetActive(false);
        }
        currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker - 1]));
    }

    public bool OpenQuestWindow()
    {
        if (!isTalking || currResponseTracker == SECOND_RESPONSE)
        {
            return false;
        }
        quest = npc.Sequences[npc.sequenceNumber].Quest;
        quest.npc = npc;
        quest.Reset();
        selectedQuestWindow.gameObject.SetActive(true);
        selectedQuestWindow.dialogueManager = this;
        selectedQuestWindow.QuestSelected(quest);
        return true;
    }

    public void CloseQuestWindow()
    {
        selectedQuestWindow.gameObject.SetActive(false);
    }

    public void QuestAccepted()
    {
        currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length - 2;
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker]));
        npc.sequenceNumber++;
    }

    public void QuestDeclined()
    {
        currResponseTracker = npc.Sequences[npc.sequenceNumber].dialogue.Length - 1;
        StartCoroutine(TypeSentence(npc.Sequences[npc.sequenceNumber].dialogue[(int)currResponseTracker]));
        currResponseTracker++;
    }

    public void HealCharacter()
    {
        if (currResponseTracker != FIRST_RESPONSE)
        {
            return;
        }
        FindObjectOfType<CharacterHealth>().FullRestore();
    }
}
