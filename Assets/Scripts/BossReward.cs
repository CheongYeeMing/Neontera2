using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReward : MonoBehaviour
{
    [SerializeField] public float expReward; // Auto added through CharacterLevel
    [SerializeField] public float goldReward; // Auto added through CharacterWallet
    [SerializeField] public Item[] itemDrops; // A single item will be dropped from the list of items with a certain percentage

    public bool rewardGiven;

    public void Start()
    {
        rewardGiven = false;
    }

    // Rewards accordingly
    public void GetReward(CharacterLevel characterLevel, CharacterWallet characterWallet)
    {
        rewardGiven = true;
        characterLevel.GainExperience(expReward);
        characterWallet.AddGold(goldReward);

    }
}
