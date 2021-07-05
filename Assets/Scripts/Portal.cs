using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal Destination;
    [SerializeField] string Location;

    bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport(GameObject Character)
    {
        StopAllCoroutines();
        StartCoroutine(Character.GetComponent<ParallaxBackgroundManager>().ChangeBackground(Destination.Location, Character, Destination));
    }
}
