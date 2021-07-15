using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundManager : MonoBehaviour
{
    [SerializeField] GameObject Intro;
    [SerializeField] GameObject IntroDetails;
    [SerializeField] GameObject Town;
    [SerializeField] GameObject TownDetails;
    [SerializeField] GameObject Forest;
    [SerializeField] GameObject ForestDetails;
    [SerializeField] GameObject Cave;
    [SerializeField] GameObject CaveDetails;
    [SerializeField] GameObject BS;
    [SerializeField] GameObject BSDetails;
    [SerializeField] GameObject SS;
    [SerializeField] GameObject SSDetails;

    [SerializeField] TransitionManager Transition;

    private const string INTRO = "Intro";
    private const string TOWN = "Town";
    private const string FOREST = "Forest";
    private const string CAVE = "Cave";
    private const string BASE_STATION = "Base Station";
    private const string SPACE_STATION = "Space Station";

    private string currentBackground;

    public void Start()
    {
        Transition.gameObject.SetActive(true);
        Intro.SetActive(false);
        IntroDetails.SetActive(false);
        Town.SetActive(false);
        TownDetails.SetActive(false);
        Forest.SetActive(false);
        ForestDetails.SetActive(false);
        Cave.SetActive(false);
        CaveDetails.SetActive(false);
        BS.SetActive(false);
        BSDetails.SetActive(false);
        SS.SetActive(false);
        SSDetails.SetActive(false);
        currentBackground = Data.location;
        SetBackground(currentBackground);
        FindObjectOfType<AudioManager>().PlayMusic(currentBackground);
    }

    public void SetBackground(string newBackground)
    {
        FindObjectOfType<AudioManager>().ChangeMusic(currentBackground, newBackground);
        currentBackground = newBackground;
        GetComponent<CharacterMovement>().location = currentBackground;
        if (newBackground == INTRO)
        {
            Intro.SetActive(true);
            IntroDetails.SetActive(true);
        }
        else if (newBackground == TOWN)
        {
            Town.SetActive(true);
            TownDetails.SetActive(true);
        }
        else if (newBackground == FOREST)
        {
            Forest.SetActive(true);
            ForestDetails.SetActive(true);
        }
        else if (newBackground == CAVE)
        {
            Cave.SetActive(true);
            CaveDetails.SetActive(true);
        }
        else if (newBackground == BASE_STATION)
        {
            BS.SetActive(true);
            BSDetails.SetActive(true);
        }
        else if (newBackground == SPACE_STATION)
        {
            SS.SetActive(true);
            SSDetails.SetActive(true);
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
            Intro.SetActive(false);
            IntroDetails.SetActive(false);
        }
        else if (currentBackground == TOWN)
        {
            Town.SetActive(false);
            TownDetails.SetActive(false);
        }
        else if (currentBackground == FOREST)
        {
            Forest.SetActive(false);
            ForestDetails.SetActive(false);
        }
        else if (currentBackground == CAVE)
        {
            Cave.SetActive(false);
            CaveDetails.SetActive(false);
        }
        else if (currentBackground == BASE_STATION)
        {
            BS.SetActive(false);
            BSDetails.SetActive(false);
        }
        else if (currentBackground == SPACE_STATION)
        {
            SS.SetActive(false);
            SSDetails.SetActive(false);
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
            Intro.SetActive(false);
            IntroDetails.SetActive(false);
        }
        else if (currentBackground == TOWN)
        {
            Town.SetActive(false);
            TownDetails.SetActive(false);
        }
        else if (currentBackground == FOREST)
        {
            Forest.SetActive(false);
            ForestDetails.SetActive(false);
        }
        else if (currentBackground == CAVE)
        {
            Cave.SetActive(false);
            CaveDetails.SetActive(false);
        }
        else if (currentBackground == BASE_STATION)
        {
            BS.SetActive(false);
            BSDetails.SetActive(false);
        }
        else if (currentBackground == SPACE_STATION)
        {
            SS.SetActive(false);
            SSDetails.SetActive(false);
        }

        SetBackground(newBackground);
    }
}
