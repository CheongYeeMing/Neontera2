using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundManager : MonoBehaviour
{
    [SerializeField] GameObject IntroBG;
    [SerializeField] GameObject IntroFG;
    [SerializeField] GameObject TownBG;
    [SerializeField] GameObject TownFG;
    [SerializeField] GameObject ForestBG;
    [SerializeField] GameObject ForestFG;
    //[SerializeField] GameObject CaveBG;
    //[SerializeField] GameObject CaveFG;
    //[SerializeField] GameObject BSBG;
    //[SerializeField] GameObject BSFG;
    //[SerializeField] GameObject SSBG;
    //[SerializeField] GameObject SSFG;

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
        IntroBG.SetActive(false);
        IntroFG.SetActive(false);
        TownBG.SetActive(false);
        TownFG.SetActive(false);
        ForestBG.SetActive(false);
        ForestFG.SetActive(false);
        //CaveBG.SetActive(false);
        //CaveFG.SetActive(false);
        //BSBG.SetActive(false);
        //BSFG.SetActive(false);
        //SSBG.SetActive(false);
        //SSFG.SetActive(false);
        currentBackground = Data.location;
        SetBackground(currentBackground);
    }

    public void SetBackground(string newBackground)
    {
        currentBackground = newBackground;
        Data.location = currentBackground;
        if (newBackground == INTRO)
        {
            IntroBG.SetActive(true);
            IntroFG.SetActive(true);
        }
        else if (newBackground == TOWN)
        {
            TownBG.SetActive(true);
            TownFG.SetActive(true);
        }
        else if (newBackground == FOREST)
        {
            ForestBG.SetActive(true);
            ForestFG.SetActive(true);
        }
        //else if (newBackground == CAVE)
        //{
        //    CaveBG.SetActive(true);
        //    CaveFG.SetActive(true);
        //}
        //else if (newBackground == BASE_STATION)
        //{
        //    BSBG.SetActive(true);
        //    BSFG.SetActive(true);
        //}
        //else if (newBackground == SPACE_STATION)
        //{
        //    SSBG.SetActive(true);
        //    SSFG.SetActive(true);
        //}
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
            IntroFG.SetActive(false);
        }
        else if (currentBackground == TOWN)
        {
            TownBG.SetActive(false);
            TownFG.SetActive(false);
        }
        else if (currentBackground == FOREST)
        {
            ForestBG.SetActive(false);
            ForestFG.SetActive(false);
        }
        //else if (currentBackground == CAVE)
        //{
        //    CaveBG.SetActive(false);
        //    CaveFG.SetActive(false);
        //}
        //else if (currentBackground == BASE_STATION)
        //{
        //    BSBG.SetActive(false);
        //    BSFG.SetActive(false);
        //}
        //else if (currentBackground == SPACE_STATION)
        //{
        //    SSBG.SetActive(false);
        //    SSFG.SetActive(false);
        //}
        SetBackground(newBackground);
    }
}
