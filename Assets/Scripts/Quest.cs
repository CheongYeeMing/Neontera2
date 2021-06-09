using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public string title;
    public string description;
    public int expReward;
    public int goldReward;
    public Item itemReward;
}
