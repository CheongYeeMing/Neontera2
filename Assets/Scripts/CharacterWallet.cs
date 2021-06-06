using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class CharacterWallet : MonoBehaviour
{
    [SerializeField] public Wallet Wallet;
    
    public float GoldAmount;

    public void Awake()
    {
        GoldAmount = 10; // to be replaced with saved stats
        UpdateWallet();
    }
    
    public void UpdateWallet()
    {
        Wallet.goldAmountText.text = GoldAmount.ToString();
    }

    public void AddGold(float goldAmount)
    {
        GoldAmount += goldAmount;
        UpdateWallet();
    }

    public void MinusGold(float goldAmount)
    {
        GoldAmount -= goldAmount;
        UpdateWallet();
    }
}
