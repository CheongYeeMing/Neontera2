using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterLevel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public Image frontExpBar;
    [SerializeField] public Image backExpBar;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public TextMeshProUGUI expText;

    [Header("Multipliers")]
    [Range(1f, 300f)]
    [SerializeField] public float additionMultiplier = 300;
    [Range(2f, 4f)]
    [SerializeField] public float powerMultiplier = 2;
    [Range(7f, 14f)]
    [SerializeField] public float divisionMultiplier = 7;

    [SerializeField] public CharacterInfoWindow charInfoWindow;

    private int level;
    private float currentExp;
    private float requiredExp;
    private float lerpTimer;
    private float delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        level = Data.level;
        currentExp = Data.currentExp;
        requiredExp = CalculateRequiredExp();
        frontExpBar.fillAmount = currentExp / requiredExp;    
        backExpBar.fillAmount = currentExp / requiredExp;
        levelText.text = "Level " + level;
        charInfoWindow.UpdateCharInfoWindow(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExpUI();
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperience(requiredExp);
        }
        if (currentExp > requiredExp)
        {
            LevelUp();
        }
        charInfoWindow.UpdateCharInfoWindow(this);
    }

    public void UpdateExpUI()
    {
        float expFraction = currentExp / requiredExp;
        float fillExp = frontExpBar.fillAmount;
        if (fillExp < expFraction)
        {
            delayTimer += Time.deltaTime;
            backExpBar.fillAmount = expFraction;
            if (delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 2;
                percentComplete = percentComplete * percentComplete;
                frontExpBar.fillAmount = Mathf.Lerp(fillExp, backExpBar.fillAmount, percentComplete);
            }
        }
        expText.text = currentExp + "/" + requiredExp;
    }

    public void GainExperience(float expGained)
    {
        currentExp += expGained;
        lerpTimer = 0f;
        delayTimer = 0f;
        Data.currentExp = currentExp;
    }

    public void LevelUp()
    {
        FindObjectOfType<AudioManager>().PlayEffect("LevelUp");
        level++;
        frontExpBar.fillAmount = 0f;
        backExpBar.fillAmount = 0f;
        currentExp = Mathf.RoundToInt(currentExp - requiredExp);
        GetComponent<CharacterHealth>().IncreaseHealth(level);
        GetComponent<CharacterAttack>().IncreaseAttack(level);
        requiredExp = CalculateRequiredExp();
        levelText.text = "Level " + level;
        Data.level = level;
        Data.currentExp = currentExp;
    }

    public int CalculateRequiredExp()
    {
        int solveForRequiredExp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredExp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredExp / 4;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetCurrentExp()
    {
        return currentExp;
    }

    public float GetRequiredExp()
    {
        return requiredExp;
    }
}
