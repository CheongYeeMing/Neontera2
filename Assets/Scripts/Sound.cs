using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    

    public bool loop;

    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public float volume;
    [HideInInspector]
    public float pitch;
}
