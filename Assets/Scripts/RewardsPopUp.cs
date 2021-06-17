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
        Transform rewardPopUpTransform = Instantiate(mob.GetComponent<MobHealth>().RewardPopUp, mob.transform.position, Quaternion.identity);
        RewardsPopUp rewardPopup = rewardPopUpTransform.GetComponent<RewardsPopUp>();
        rewardPopup.Setup(mob.GetComponent<MobReward>().expReward, mob.GetComponent<MobReward>().goldReward);
        return rewardPopup;
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
