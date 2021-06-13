using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Health
{
    public void TakeDamage(float damage);

    public void Die();

    public void HurtComplete();
}
