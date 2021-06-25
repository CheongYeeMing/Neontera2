using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible for the Exp, Gold, Item reward upon Die();
public class MobReward : MonoBehaviour
{
    [SerializeField] public float expReward; // Auto added through CharacterLevel
    [SerializeField] public float goldReward; // Auto added through CharacterWallet

    protected bool isRewardGiven;

    public void Start()
    {
        isRewardGiven = false;
    }

    // Rewards accordingly
    public virtual void GetReward(CharacterLevel characterLevel, CharacterWallet characterWallet)
    {
        isRewardGiven = true;
        characterLevel.GainExperience(expReward);
        characterWallet.AddGold(goldReward);
    }

    public bool GetIsRewardGiven()
    {
        return isRewardGiven;
    }

    public void SetIsRewardGiven(bool isRewardGiven)
    {
        this.isRewardGiven = isRewardGiven;        
    }
}
