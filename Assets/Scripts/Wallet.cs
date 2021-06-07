using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    public Text goldAmountText;

    private void OnValidate()
    {
        goldAmountText = GetComponentInChildren<Text>();
    }
}
