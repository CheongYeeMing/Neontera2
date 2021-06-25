using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class CharacterWallet : MonoBehaviour
{
    [SerializeField] private List<Wallet> Wallet;
    
    private float GoldAmount;

    public void Awake()
    {
        GoldAmount = 10; // to be replaced with saved stats
        UpdateWallet();
    }
    
    public void UpdateWallet()
    {
        foreach (Wallet wallet in Wallet)
        {
            wallet.goldAmountText.text = GoldAmount.ToString();
        }
        
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

    public bool HasEnoughGold(float goldAmount)
    {
        return goldAmount <= GetGoldAmount();
    }

    public List<Wallet> GetListWallet()
    {
        return Wallet;
    }

    public float GetGoldAmount()
    {
        return GoldAmount;
    }
}
