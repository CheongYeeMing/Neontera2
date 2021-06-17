using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardsPopUp : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float disappearTimer;
    private Color textColor;

    public void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static RewardsPopUp Create(GameObject mob)
    {
        Transform rewardPopUpTransform;
        RewardsPopUp rewardPopup;
        MobHealth mobHealth;
        if (mob.TryGetComponent<MobHealth>(out mobHealth))
        {
            rewardPopUpTransform = Instantiate(mobHealth.RewardPopUp, mob.transform.position, Quaternion.identity);
            rewardPopup = rewardPopUpTransform.GetComponent<RewardsPopUp>();
            rewardPopup.Setup(mob.GetComponent<MobReward>().expReward, mob.GetComponent<MobReward>().goldReward);
            return rewardPopup;
        }
        else
        {
            BossHealth bossHealth;
            mob.TryGetComponent<BossHealth>(out bossHealth);
            rewardPopUpTransform = Instantiate(bossHealth.RewardPopUp, mob.transform.position, Quaternion.identity);
            rewardPopup = rewardPopUpTransform.GetComponent<RewardsPopUp>();
            rewardPopup.Setup(mob.GetComponent<BossReward>().expReward, mob.GetComponent<BossReward>().goldReward);
            return rewardPopup;
        }
    }

    public void Setup(float expAmount, float goldAmount)
    {
        textMesh.SetText(expAmount + " exp\n" + goldAmount + " gold");
        textColor = textMesh.color;
        disappearTimer = 2f;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 1f) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
