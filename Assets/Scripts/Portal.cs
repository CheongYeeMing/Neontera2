using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        TeleportToTown();
    }

    public void TeleportToTown()
    {
        SceneManager.LoadScene(3);
    }

    public void TeleportToForest()
    {
        SceneManager.LoadScene(4);
    }
}
