using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] public bool canRespawn;
    [SerializeField] public float respawnTime;

    private float deathTimer;

    // Update is called once per frame
    void Update()
    {
        if (!canRespawn) return;
        if (gameObject.GetComponent<MobHealth>().IsDead())
        {
            if (deathTimer > respawnTime)
            {
                Respawn();
            }
            deathTimer += Time.deltaTime;
        }
    }

    public void Respawn()
    {
        gameObject.transform.position = gameObject.GetComponent<MobMovement>().GetSpawnPoint();
        gameObject.GetComponent<MobHealth>().Start();
        gameObject.GetComponent<MobReward>().Start();
        gameObject.GetComponent<MobAnimation>().Start();
    }

    public void SetDeathTimer(float deathTimer)
    {
        this.deathTimer = deathTimer;
    }
}
