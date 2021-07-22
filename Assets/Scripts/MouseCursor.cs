using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{   public GameObject trialEffect;
    public float timeBtwSpawn = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        if(timeBtwSpawn <= 0)
        {
            Instantiate(trialEffect, cursorPos, Quaternion.identity);
            timeBtwSpawn = 0.03f;
        } 
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}
