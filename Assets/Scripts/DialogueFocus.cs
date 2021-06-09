using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFocus : MonoBehaviour
{
    public bool ZoomActive = false;

    public GameObject character;
    [SerializeField] public GameObject dialogueZoom;

    [SerializeField] public Cinemachine.CinemachineVirtualCamera cam;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (ZoomActive)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, 3, speed);
            cam.Follow = dialogueZoom.transform;
        }
        else
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, 5, speed);
            cam.Follow = character.transform;
        }
    }

    public void ToggleZoom()
    {
        ZoomActive = !ZoomActive;
    }
}
