using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CharacterWallet : MonoBehaviour
{
    private const float PERCENT = 10;

    [SerializeField] private List<Wallet> Wallet;
    
    private float goldAmount;

    private void Awake()
    {
        LoadCharacterWalletData();
        UpdateWallet();
    }

    private void LoadCharacterWalletData()
    {
        goldAmount = Data.gold;
    }
    
    private void UpdateWallet()
    {
        foreach (Wallet wallet in Wallet)
        {
            wallet.goldAmountText.text = goldAmount.ToString();
        }
        Data.gold = goldAmount;
    }

    public void Revive()
    {
        MinusGold(goldAmount * PERCENT / 100);
    }

    public void AddGold(float goldAmount)
    {
        FindObjectOfType<AudioManager>().StopEffect("GainGold");
        FindObjectOfType<AudioManager>().PlayEffect("GainGold");
        this.goldAmount += goldAmount;
        UpdateWallet();
    }

    public void MinusGold(float goldAmount)
    {
        this.goldAmount -= goldAmount;
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
        return goldAmount;
    }
}
