using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public float healAmount;

    public float AttackPercentBonus;
    public float HealthPercentBonus;
    public float SpeedPercentBonus;
    public float duration;

    public bool isConsumed;

    public ConsumableType consumableType;


    public void Update()
    {
        
    }

    public void Consume(Character c)
    {
        FindObjectOfType<AudioManager>().PlayEffect("ConsumeItem");
        if (consumableType == ConsumableType.Instant)
        {
            Heal(c);
        }
        else if (consumableType == ConsumableType.FadeOverTime)
        {
            Buff(c);
            isConsumed = true;
        }
    }

    public void Heal(Character c)
    {
        FindObjectOfType<AudioManager>().PlayEffect("CharacterHeal");
        c.gameObject.GetComponent<CharacterHealth>().RestoreHealth(healAmount);
    }

    public void Buff(Character c)
    {
        if (AttackPercentBonus != 0)
        {
            c.GetAttack().AddModifier(new StatModifier(AttackPercentBonus, StatModType.PercentMult, this));
        }
        if (HealthPercentBonus != 0)
        {
            c.GetHealth().AddModifier(new StatModifier(HealthPercentBonus, StatModType.PercentMult, this));
        }
        if (SpeedPercentBonus != 0)
        {
            c.GetSpeed().AddModifier(new StatModifier(SpeedPercentBonus, StatModType.PercentMult, this));
        }
    }

    public void Debuff(Character c)
    {
        c.GetAttack().RemoveAllModifiersFromSource(this);
        c.GetHealth().RemoveAllModifiersFromSource(this);
        c.GetSpeed().RemoveAllModifiersFromSource(this);
    }
}

public enum ConsumableType
{
    Instant,
    FadeOverTime
}
