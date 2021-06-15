using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealingPopUp : MonoBehaviour
{ 
    public TextMeshPro textMesh;
    public float disappearTimer;
    private Color textColor;

    public void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static HealingPopUp Create(GameObject mob, float heal)
    {
        Transform healingPopUpTransform = Instantiate(mob.GetComponent<MobHealth>().HealingPopup, mob.transform.position, Quaternion.identity);
        HealingPopUp healingPopup = healingPopUpTransform.GetComponent<HealingPopUp>();
        healingPopup.Setup(heal);
        return healingPopup;
    }

    public void Setup(float healAmount)
    {
        textMesh.SetText(healAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 1f;
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