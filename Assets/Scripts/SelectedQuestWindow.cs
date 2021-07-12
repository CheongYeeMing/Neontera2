using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedQuestWindow : MonoBehaviour
{
    [SerializeField] public GameObject AcceptedQuestWindow;
    [SerializeField] public QuestList questList;
    [SerializeField] public Inventory inventory;

    public Quest quest;
    public Item item;
    public DialogueManager dialogueManager;

    [SerializeField] public Text questName;
    [SerializeField] public Text questDescription;
    [SerializeField] public Text questCriteria;
    [SerializeField] public Text questExpReward;
    [SerializeField] public Text questGoldReward;
    [SerializeField] public Text questItemReward;
    [SerializeField] public Button AbandonQuestButton;
    [SerializeField] public Button ContinueQuestButton;
    [SerializeField] public Button CompleteQuestButton;
    [SerializeField] public Button AcceptQuestButton;
    [SerializeField] public Button DeclineQuestButton;

    public void QuestSelected(Quest quest)
    {
        gameObject.SetActive(true);
        this.quest = quest;
        questName.text = quest.title;
        questDescription.text = quest.description;
        questCriteria.text = quest.questCriteria.action + ": " + quest.questCriteria.currentAmount + "/" + quest.questCriteria.requiredAmount;
        questExpReward.text = quest.expReward.ToString();
        questGoldReward.text = quest.goldReward.ToString();
        if (quest.itemReward != null)
            questItemReward.text = quest.itemReward.name.ToString();
    }

    public void Update()
    {
        if (quest.status == Quest.Status.WAITING)
        {
            AbandonQuestButton.gameObject.SetActive(false);
            ContinueQuestButton.gameObject.SetActive(false);
            CompleteQuestButton.gameObject.SetActive(false);
            AcceptQuestButton.gameObject.SetActive(true);
            DeclineQuestButton.gameObject.SetActive(true);
        }
        else if (quest.status == Quest.Status.ONGOING)
        {
            AbandonQuestButton.gameObject.SetActive(true);
            ContinueQuestButton.gameObject.SetActive(true);
            CompleteQuestButton.gameObject.SetActive(false);
            AcceptQuestButton.gameObject.SetActive(false);
            DeclineQuestButton.gameObject.SetActive(false);
        }
        else if (quest.status == Quest.Status.COMPLETED)
        {
            AbandonQuestButton.gameObject.SetActive(true);
            ContinueQuestButton.gameObject.SetActive(false);
            CompleteQuestButton.gameObject.SetActive(true);
            AcceptQuestButton.gameObject.SetActive(false);
            DeclineQuestButton.gameObject.SetActive(false);
        }
    }

    public void AcceptQuest()
    {
        FindObjectOfType<AudioManager>().PlayEffect("QuestAccepted");
        dialogueManager.QuestAccepted();
        questList.AddQuest(quest);
        quest.status = Quest.Status.ONGOING;
        gameObject.SetActive(false);
        if (quest.questItem != null)
            item = Instantiate(quest.questItem, quest.location, Quaternion.identity);
    }

    public void DeclineQuest()
    {
        dialogueManager.QuestDeclined();
        gameObject.SetActive(false);
    }

    public void AbandonQuest()
    {
        if (quest.questCriteria.criteriaType == CriteriaType.Collect && item != null)
            inventory.RemoveItem(item);
        quest.Start();
        questList.RemoveQuest(quest);
        gameObject.SetActive(false);
        if (item != null)
        {
            Destroy(item.gameObject);
        }
        quest.npc.sequenceNumber--;

    }

    public void ContinueQuest()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        gameObject.SetActive(false);
    }

    public void CompleteQuest()
    {
        FindObjectOfType<AudioManager>().StopEffect("QuestComplete");
        FindObjectOfType<AudioManager>().PlayEffect("QuestComplete");
        if (quest.questCriteria.criteriaType == CriteriaType.Collect && item != null)
            inventory.RemoveItem(item);
        quest.npc.sequenceNumber++;
        questList.RemoveQuest(quest);
        FindObjectOfType<CharacterLevel>().GainExperience(quest.expReward);
        FindObjectOfType<CharacterWallet>().AddGold(quest.goldReward);
        if (quest.itemReward != null)
            FindObjectOfType<Inventory>().AddItem(quest.itemReward);
        gameObject.SetActive(false);
        
        // Some Quest to be reset
        if (quest.canRepeat == true)
        {
            quest.FinalReset();
        }
        // Some Quest can only be done once then move on to next Quest already

    }
}
