using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CharacterWallet : MonoBehaviour
{
    private const float PERCENT = 10;

    [SerializeField] private List<Wallet> Wallet;
    
    private float GoldAmount;

    private void Awake()
    {
        LoadCharacterWalletData();
        UpdateWallet();
    }

    private void LoadCharacterWalletData()
    {
        GoldAmount = Data.gold;
    }
    
    private void UpdateWallet()
    {
        foreach (Wallet wallet in Wallet)
        {
            wallet.goldAmountText.text = GoldAmount.ToString();
        }
        Data.gold = GoldAmount;
    }

    public void Revive()
    {
        MinusGold(GoldAmount * PERCENT / 100);
    }

    public void AddGold(float goldAmount)
    {
        FindObjectOfType<AudioManager>().StopEffect("GainGold");
        FindObjectOfType<AudioManager>().PlayEffect("GainGold");
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
