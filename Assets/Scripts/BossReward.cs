using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReward : MobReward
{
    // Boss Can drop certain items
    [SerializeField] public Item[] itemDrops; // A single item will be dropped from the list of items with a certain percentage

    public override void GetReward(CharacterLevel characterLevel, CharacterWallet characterWallet)
    {
        if (isRewardGiven) return;
        isRewardGiven = true;
        characterLevel.GainExperience(expReward);
        characterWallet.AddGold(goldReward);
    }
}
