using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monologue : MonoBehaviour
{
    [SerializeField] NPC npc;
    [SerializeField] DialogueFocus dialogueFocus;
    [SerializeField] Character character;
    [SerializeField] GameObject examineWindow;
    [SerializeField] Image examineImage;
    [SerializeField] Text examineText;
    [SerializeField] TextMeshProUGUI enterToContinue;
    [SerializeField] bool isThoughts;

    [SerializeField] List<string> text;

    [SerializeField] Item key;

    [SerializeField] bool requireKey;

    bool isExamining;
    [SerializeField] bool triggerAgain;
    int currTextNumber;

    [SerializeField] bool activateBoss;
    [SerializeField] GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        currTextNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsExamining()) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<AudioManager>().StopEffect("RetroClick");
            FindObjectOfType<AudioManager>().PlayEffect("RetroClick");
            if (requireKey)
            {
                if (!HasKey())
                {
                    enterToContinue.gameObject.SetActive(false);
                    StopAllCoroutines();
                    ToggleMonologue();
                    character.gameObject.transform.position += new Vector3(-Mathf.Sign(character.transform.localScale.x) * 0.1f, 0, 0); 
                }
                else if (HasKey())
                {
                    if (currTextNumber < text.Count)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TypeSentence(text[currTextNumber++]));
                    }
                    else
                    {
                        enterToContinue.gameObject.SetActive(false);
                        StopAllCoroutines();
                        ToggleMonologue();
                        if (!triggerAgain)
                        {
                            character.inventory.RemoveItem(key);
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                if (currTextNumber < text.Count)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(text[currTextNumber++]));
                }
                else
                {
                    enterToContinue.gameObject.SetActive(false);
                    StopAllCoroutines();
                    ToggleMonologue();
                    if (!triggerAgain)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void ToggleMonologue()
    {
        currTextNumber = 0;
        dialogueFocus.ToggleZoom();
        if (isExamining)
        {
            examineWindow.SetActive(false);
            isExamining = false;
            if (activateBoss)
            {
                boss.GetComponent<BossAttack>().enabled = true;
                boss.GetComponent<BossHealth>().enabled = true;
                boss.GetComponent<BossMovement>().enabled = true;
            }
        }
        else
        {
            character.GetComponent<CharacterAnimation>().ChangeAnimationState("Idle");
            character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            FindObjectOfType<AudioManager>().StopEffect("Run");
            FindObjectOfType<AudioManager>().StopEffect("DialogueMonologue");
            FindObjectOfType<AudioManager>().PlayEffect("DialogueMonologue");
            if (!isThoughts)
                examineImage.sprite = npc.icon;
            // Write description text on the right side of image
            StopAllCoroutines();
            StartCoroutine(TypeSentence(text[currTextNumber++]));
            // Display an Examine Window
            examineWindow.SetActive(true);
            // Enable the boolean
            isExamining = true;
            if (activateBoss)
            {
                boss.gameObject.SetActive(true);
                boss.GetComponent<BossAttack>().enabled = false;
                boss.GetComponent<BossHealth>().enabled = false;
                boss.GetComponent<BossMovement>().enabled = false;
            }
        }
    }


    public IEnumerator TypeSentence(string sentence)
    {
        enterToContinue.gameObject.SetActive(false);
        examineText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            examineText.text += letter;
            yield return new WaitForSeconds(0.015f);
        }
        enterToContinue.gameObject.SetActive(true);
    }

    public bool IsExamining()
    {
        return isExamining;
    }

    public bool HasKey()
    {
        return character.inventory.ContainsItem(key);
    }
}
