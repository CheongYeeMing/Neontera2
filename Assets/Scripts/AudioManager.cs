using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    private const float SOUND_PITCH = 1;

    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider EffectsVolumeSlider;

    private float previousMusicVolume;
    private float previousEffectsVolume;

    public Sound[] musicList;
    public Sound[] effectsList;

    // Start is called before the first frame update
    void Awake()
    {
        GetSavedVolumeData();
        SetSavedVolumeData();
        Initialise(MusicVolumeSlider, musicList);
        Initialise(EffectsVolumeSlider, effectsList);
    }

    private void GetSavedVolumeData()
    {
        MusicVolumeSlider.value = Data.musicVolume;
        EffectsVolumeSlider.value = Data.effectsVolume;
    }

    private void SetSavedVolumeData()
    {
        previousMusicVolume = MusicVolumeSlider.value;
        previousEffectsVolume = EffectsVolumeSlider.value;
    }

    private void Initialise(Slider slider, Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = SOUND_PITCH;
            s.source.loop = s.loop;

            s.source.volume = slider.value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicVolumeChanged())
        {
            UpdateMusicVolume();
        }
        if (EffectsVolumeChanged())
        {
            UpdateEffectsVolume();
        }
    }

    private bool MusicVolumeChanged()
    {
        return MusicVolumeSlider.value != previousMusicVolume;
    }

    private bool EffectsVolumeChanged()
    {
        return EffectsVolumeSlider.value != previousEffectsVolume;
    }

    private void UpdateMusicVolume()
    {
        previousMusicVolume = MusicVolumeSlider.value;
        Data.musicVolume = previousMusicVolume;
        foreach(Sound s in musicList)
        {
            s.source.volume = previousMusicVolume;
        }
    }

    private void UpdateEffectsVolume()
    {
        previousEffectsVolume = EffectsVolumeSlider.value;
        Data.effectsVolume = previousEffectsVolume;
        foreach (Sound s in effectsList)
        {
            s.source.volume = previousEffectsVolume;
        }
    }

    public void ChangeMusic(string prevBG, string currBG)
    {
        StartCoroutine(UpdateBGMPortal(prevBG, currBG));
    }

    public IEnumerator UpdateBGMPortal(string prevBG, string currBG)
    {
        // Searching for previous and current Music
        Sound prev = Array.Find(musicList, sound => sound.name == prevBG);
        Sound curr = Array.Find(musicList, sound => sound.name == currBG);

        // Gradually reduce volume of previous Music
        for (int i = (int)(prev.source.volume * 100); i > 0; i--)
        {
            prev.source.volume -= 0.01f;
            yield return null;
        }

        // Swapping music
        prev.source.Stop();
        curr.source.volume = 0;
        curr.source.Play();

        // Gradually increase volume of current Music
        for (int i = 0; i < MusicVolumeSlider.value; i++)
        {
            curr.source.volume += 0.01f;
        }

        curr.source.volume = MusicVolumeSlider.value;
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicList, sound => sound.name == name);
        if (s == null || s.source.isPlaying) return;
        s.source.Play();
    }

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effectsList, sound => sound.name == name);
        if (s == null || s.source.isPlaying) return;
        s.source.Play();
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicList, sound => sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }

    public void StopEffect(string name)
    {
        Sound s = Array.Find(effectsList, sound => sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }
}
