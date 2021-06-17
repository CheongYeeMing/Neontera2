using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{ 
    public TextMeshPro textMesh;
    public float disappearTimer;
    private Color textColor;

    public void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static DamagePopUp Create(GameObject mob, float damage)
    {
        Transform damagePopUpTransform;
        DamagePopUp damagePopUp;
        MobHealth mobHealth;
        if (mob.TryGetComponent<MobHealth>(out mobHealth))
        {
            damagePopUpTransform = Instantiate(mobHealth.DamagePopup, mob.transform.position, Quaternion.identity);
            damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup(damage);
            return damagePopUp;
        }
        else
        {
            BossHealth bossHealth;
            mob.TryGetComponent<BossHealth>(out bossHealth);
            damagePopUpTransform = Instantiate(bossHealth.DamagePopup, mob.transform.position, Quaternion.identity);
            damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup(damage);
            return damagePopUp;
        }
    }

    public void Setup(float damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
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
