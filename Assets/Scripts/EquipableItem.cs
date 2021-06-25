using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Armor,
    Boots,
    Weapon
}
public class EquipableItem : Item
{
    public int AttackBonus;
    public int HealthBonus;
    public int SpeedBonus;
    [Space]
    public float AttackPercentBonus;
    public float HealthPercentBonus;
    public float SpeedPercentBonus;
    [Space]
    public EquipmentType EquipmentType;

    public bool isEquipped;

    public void Equip(Character c)
    {
        isEquipped = true;
        if (AttackBonus != 0)
        {
            c.GetAttack().AddModifier(new StatModifier(AttackBonus, StatModType.Flat, this));
        }
        if (HealthBonus != 0)
        {
            c.GetHealth().AddModifier(new StatModifier(HealthBonus, StatModType.Flat, this));
        }
        if (SpeedBonus != 0)
        {
            c.GetSpeed().AddModifier(new StatModifier(SpeedBonus, StatModType.Flat, this));
        }
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

    public void Unequip(Character c)
    {
        isEquipped = false;
        c.GetAttack().RemoveAllModifiersFromSource(this);
        c.GetHealth().RemoveAllModifiersFromSource(this);
        c.GetSpeed().RemoveAllModifiersFromSource(this);
    }
}
