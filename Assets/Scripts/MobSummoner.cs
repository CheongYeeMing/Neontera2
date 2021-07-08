using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobSummoner : MonoBehaviour
{
    [SerializeField] public Transform prefab;

    public abstract void Summon();
}
