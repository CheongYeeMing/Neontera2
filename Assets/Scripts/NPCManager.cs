using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] NPC JohnsonIntro;
    [SerializeField] NPC DrAaronIntro;
    [SerializeField] NPC JohnsonTown;
    [SerializeField] NPC DrAaronTown;
    [SerializeField] NPC JeanneTown;
    [SerializeField] NPC MartinTown;
    [SerializeField] NPC LloydForest;
    [SerializeField] NPC TrinaCave;

    [SerializeField] NPC Nicole;
    [SerializeField] NPC Wilson;

    [SerializeField] public NPC QuestList;
    [SerializeField] QuestList questList;

    public void Start()
    {
        JohnsonIntro.sequenceNumber = Data.JohnsonIntro;
        DrAaronIntro.sequenceNumber = Data.DrAaronIntro;
        JohnsonTown.sequenceNumber = Data.JohnsonTown;
        DrAaronTown.sequenceNumber = Data.DrAaronTown;
        JeanneTown.sequenceNumber = Data.JeanneTown;
        MartinTown.sequenceNumber = Data.MartinTown;
        LloydForest.sequenceNumber = Data.LloydForest;
        TrinaCave.sequenceNumber = Data.TrinaCave;

        Nicole.sequenceNumber = Data.Nicole;
        Wilson.sequenceNumber = Data.Wilson;

        for (int i = 0; i < Data.quests.Count; i++)
        {
            Quest quest = QuestList.Sequences[Data.quests[i]].Quest;
            quest.Reset();
            quest.status = Quest.Status.ONGOING;
            quest.questCriteria.currentAmount = Data.questProgress[i];
            questList.AddQuest(quest);
        }
    }
    public void Save()
    {
        Data.JohnsonIntro = JohnsonIntro.sequenceNumber;
        Data.DrAaronIntro = DrAaronIntro.sequenceNumber;
        Data.JohnsonTown = JohnsonTown.sequenceNumber;
        Data.DrAaronTown = DrAaronTown.sequenceNumber;
        Data.JeanneTown = JeanneTown.sequenceNumber;
        Data.MartinTown = MartinTown.sequenceNumber;
        Data.LloydForest = LloydForest.sequenceNumber;
        Data.TrinaCave = TrinaCave.sequenceNumber;

        Data.Nicole = Nicole.sequenceNumber;
        Data.Wilson = Wilson.sequenceNumber;
    }
}
