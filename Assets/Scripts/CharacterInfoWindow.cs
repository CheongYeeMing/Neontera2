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
        charLevel.text = "Level " + characterLevel.GetLevel().ToString();
        charExp.text = "Exp: " + characterLevel.GetCurrentExp() + "/" + characterLevel.GetRequiredExp();
    }

}
