using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public bool Shaking;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Shaking = false;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake()
    {
        CinemachineBasicMultiChannelPerlin cbmpc = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmpc.m_AmplitudeGain = 2f;
    }

    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmpc = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmpc.m_AmplitudeGain = 0;
    }

    public void Hit()
    {
        if (Shaking) return;
        StartCoroutine(takeDamage());
    }

    public IEnumerator takeDamage()
    {
        Shaking = true;
        yield return new WaitForSeconds(0.2f);
        Shaking = false;
    }

    public void Update()
    {
        if (Shaking) Shake();
        else StopShake();
    }
}
