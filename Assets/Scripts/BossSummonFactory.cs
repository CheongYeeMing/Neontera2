using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossSummonFactory
{
    // Instantiate Skill 
    public void Summon(string skill);
}
