using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        ps = GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
