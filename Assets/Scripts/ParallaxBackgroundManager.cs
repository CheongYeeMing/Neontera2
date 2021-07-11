using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundManager : MonoBehaviour
{
    [SerializeField] GameObject IntroBG;
    [SerializeField] GameObject TownBG;
    [SerializeField] GameObject ForestBG;
    [SerializeField] GameObject CaveBG;
    [SerializeField] GameObject BSBG;
    [SerializeField] GameObject SSBG;

    [SerializeField] TransitionManager Transition;

    private const string INTRO = "Intro";
    private const string TOWN = "Town";
    private const string FOREST = "Forest";
    private const string CAVE = "Cave";
    private const string BASE_STATION = "Base Station";
    private const string SPACE_STATION = "Space Station";

    private string currentBackground;

    public void OnValidate()
    {
        Transition.gameObject.SetActive(true);
        IntroBG.SetActive(false);
        TownBG.SetActive(false);
        ForestBG.SetActive(false);
        CaveBG.SetActive(false);
        BSBG.SetActive(false);
        SSBG.SetActive(false);
        currentBackground = Data.location;
        SetBackground(currentBackground);
        
    }

    public void Start()
    {
        FindObjectOfType<AudioManager>().PlayMusic(currentBackground);
    }

    public void SetBackground(string newBackground)
    {
        FindObjectOfType<AudioManager>().ChangeMusic(currentBackground, newBackground);
        currentBackground = newBackground;
        Data.location = currentBackground;
        if (newBackground == INTRO)
        {
            IntroBG.SetActive(true);
        }
        else if (newBackground == TOWN)
        {
            TownBG.SetActive(true);
        }
        else if (newBackground == FOREST)
        {
            ForestBG.SetActive(true);
        }
        else if (newBackground == CAVE)
        {
            CaveBG.SetActive(true);
        }
        else if (newBackground == BASE_STATION)
        {
            BSBG.SetActive(true);
        }
        else if (newBackground == SPACE_STATION)
        {
            SSBG.SetActive(true);
        }
        Transition.Deactivate();
    }

    public IEnumerator ChangeBackground(string newBackground, GameObject Character, Portal Destination)
    {
        Transition.Activate();
        yield return new WaitForSeconds(1.3f);
        Character.transform.position = Destination.transform.position;
        if (currentBackground == INTRO)
        {
            IntroBG.SetActive(false);
        }
        else if (currentBackground == TOWN)
        {
            TownBG.SetActive(false);
        }
        else if (currentBackground == FOREST)
        {
            ForestBG.SetActive(false);
        }
        else if (currentBackground == CAVE)
        {
            CaveBG.SetActive(false);
        }
        else if (currentBackground == BASE_STATION)
        {
            BSBG.SetActive(false);
        }
        else if (currentBackground == SPACE_STATION)
        {
            SSBG.SetActive(false);
        }
        
        SetBackground(newBackground);
    }

    public IEnumerator Teleport(string newBackground, GameObject Character, Vector2 Destination)
    {
        Transition.Activate();
        yield return new WaitForSeconds(1.3f);
        Character.transform.position = Destination;
        if (currentBackground == INTRO)
        {
            IntroBG.SetActive(false);
        }
        else if (currentBackground == TOWN)
        {
            TownBG.SetActive(false);
        }
        else if (currentBackground == FOREST)
        {
            ForestBG.SetActive(false);
        }
        else if (currentBackground == CAVE)
        {
            CaveBG.SetActive(false);
        }
        else if (currentBackground == BASE_STATION)
        {
            BSBG.SetActive(false);
        }
        else if (currentBackground == SPACE_STATION)
        {
            SSBG.SetActive(false);
        }

        SetBackground(newBackground);
    }
}
