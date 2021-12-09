using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    private const int ZERO = 0;
    private const string EXP_TEXT = "Exp: ";
    private const string LEVEL_TEXT = "Level ";
    //private const string USERNAME_TEXT = "Username";
    private const string SEPARATOR = "/";

    [SerializeField] public Text charExp;
    [SerializeField] public Text charLevel;
    //[SerializeField] public Text charName;

    public void Start()
    {
        //charName.text = USERNAME_TEXT;
        charLevel.text = LEVEL_TEXT + ZERO;
        charExp.text = EXP_TEXT + ZERO + SEPARATOR + ZERO;
    }

    public void UpdateCharInfoWindow(CharacterLevel characterLevel)
    {
        charLevel.text = LEVEL_TEXT + characterLevel.GetLevel();
        charExp.text = EXP_TEXT + characterLevel.GetCurrentExp() + SEPARATOR + characterLevel.GetRequiredExp();
    }

}
