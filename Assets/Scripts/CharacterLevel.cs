using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterLevel : MonoBehaviour
{
    private const float EXP_LERP_DELAY = 2f;
    private const string LEVEL_TEXT = "Level ";
    private const string SEPARATOR = "/";
    [Header("UI")]
    [SerializeField] private Image frontExpBar;
    [SerializeField] private Image backExpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("Multipliers")]
    [Range(1f, 300f)]
    [SerializeField] private float additionMultiplier = 300;
    [Range(2f, 4f)]
    [SerializeField] private float powerMultiplier = 2;
    [Range(7f, 14f)]
    [SerializeField] private float divisionMultiplier = 7;

    [SerializeField] private CharacterInfoWindow charInfoWindow;

    private float currentExp;
    private float requiredExp;
    private float lerpTimer;
    private float delayTimer;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        level = Data.level;
        currentExp = Data.currentExp;
        requiredExp = CalculateRequiredExp();
        frontExpBar.fillAmount = currentExp / requiredExp;    
        backExpBar.fillAmount = currentExp / requiredExp;
        levelText.text = LEVEL_TEXT + level;
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
            if (delayTimer > EXP_LERP_DELAY)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 2;
                percentComplete = percentComplete * percentComplete;
                frontExpBar.fillAmount = Mathf.Lerp(fillExp, backExpBar.fillAmount, percentComplete);
            }
        }
        expText.text = currentExp + SEPARATOR + requiredExp;
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
        levelText.text = LEVEL_TEXT + level;
        Data.level = level;
        Data.currentExp = currentExp;
    }

    public int CalculateRequiredExp()
    {
        int requiredExp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            requiredExp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return requiredExp / 4;
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
