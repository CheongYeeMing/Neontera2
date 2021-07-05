using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSummoner : MonoBehaviour
{
    [SerializeField] public Transform prefab;

    public abstract void Summon();
}
