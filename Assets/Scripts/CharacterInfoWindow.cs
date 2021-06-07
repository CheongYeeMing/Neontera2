using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    [SerializeField] public Text charName;
    [SerializeField] public Text charLevel;
    [SerializeField] public Text charExp;

    public void Start()
    {
        charName.text = "Username";
        charLevel.text = "Level " + 0;
        charExp.text = "Exp: " + 0 +"/"+ 0;
    }

    public void UpdateCharInfoWindow(CharacterLevel characterLevel)
    {
        charLevel.text = "Level " + characterLevel.level.ToString();
        charExp.text = "Exp: " + characterLevel.currentExp + "/" + characterLevel.requiredExp;
    }

}
