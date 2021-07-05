using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] public bool canRespawn;
    [SerializeField] public float respawnTime;

    private float deathTimer;

    // Update is called once per frame
    void Update()
    {
        if (!canRespawn) return;
        if (gameObject.GetComponent<BossHealth>().IsDead())
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
        gameObject.transform.position = gameObject.GetComponent<BossMovement>().GetSpawnPoint();
        gameObject.GetComponent<BossHealth>().Start();
        gameObject.GetComponent<BossReward>().Start();
        gameObject.GetComponent<BossAnimation>().Start();
    }

    public void SetDeathTimer(float deathTimer)
    {
        this.deathTimer = deathTimer;
    }
}
