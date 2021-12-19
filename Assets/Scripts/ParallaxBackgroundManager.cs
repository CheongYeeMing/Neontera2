using System.Collections;
using UnityEngine;

public class ParallaxBackgroundManager : MonoBehaviour
{
    private const string INTRO = "Intro";
    private const string TOWN = "Town";
    private const string FOREST = "Forest";
    private const string CAVE = "Cave";
    private const string BASE_STATION = "Base Station";
    private const string SPACE_STATION = "Space Station";
    private const string SECRET_AREA_1 = "Secret Area 1";
    private const string SECRET_AREA_2 = "Secret Area 2";

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
    [SerializeField] GameObject SecretArea1;
    [SerializeField] GameObject SecretArea1Details;
    [SerializeField] GameObject SecretArea2;
    [SerializeField] GameObject SecretArea2Details;

    [SerializeField] TransitionManager Transition;

    public bool isTeleporting;
    private string currentBackground;

    private void Start()
    {
        Transition.gameObject.SetActive(true);
        DeactivateIntro();
        DeactivateTown();
        DeactivateForest();
        DeactivateCave();
        DeactivateBaseStation();
        DeactivateSpaceStation();
        DeactivateSecretArea1();
        SecretArea2.SetActive(false);
        SecretArea2Details.SetActive(false);
        currentBackground = Data.location;
        SetBackground(currentBackground);
        FindObjectOfType<AudioManager>().PlayMusic(currentBackground);
        isTeleporting = false;
    }

    private void DeactivateIntro()
    {
        Intro.SetActive(false);
        IntroDetails.SetActive(false);
    }

    private void DeactivateTown()
    {
        Town.SetActive(false);
        TownDetails.SetActive(false);
    }

    private void DeactivateForest()
    {
        Forest.SetActive(false);
        ForestDetails.SetActive(false);
    }

    private void DeactivateCave()
    {
        Cave.SetActive(false);
        CaveDetails.SetActive(false);
    }

    private void DeactivateBaseStation()
    {
        BS.SetActive(false);
        BSDetails.SetActive(false);
    }

    private void DeactivateSpaceStation()
    {
        SS.SetActive(false);
        SSDetails.SetActive(false);
    }

    private void DeactivateSecretArea1()
    {
        SecretArea1.SetActive(false);
        SecretArea1Details.SetActive(false);
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
        else if (newBackground == SECRET_AREA_1)
        {
            SecretArea1.SetActive(true);
            SecretArea1Details.SetActive(true);
        }
        else if (newBackground == SECRET_AREA_2)
        {
            SecretArea2.SetActive(true);
            SecretArea2Details.SetActive(true);
        }
        Transition.Deactivate();
        if (GetComponent<CharacterHealth>().IsDead())
        {
            GetComponent<CharacterHealth>().Revive();
            GetComponent<CharacterWallet>().MinusGold(GetComponent<CharacterWallet>().GetGoldAmount() / 10);
        }
        isTeleporting = false;
    }

    public void OffBackground()
    {
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
        else if (currentBackground == SECRET_AREA_1)
        {
            SecretArea1.SetActive(false);
            SecretArea1Details.SetActive(false);
        }
        else if (currentBackground == SECRET_AREA_2)
        {
            SecretArea2.SetActive(false);
            SecretArea2Details.SetActive(false);
        }
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
        else if (currentBackground == SECRET_AREA_1)
        {
            SecretArea1.SetActive(false);
            SecretArea1Details.SetActive(false);
        }
        else if (currentBackground == SECRET_AREA_2)
        {
            SecretArea2.SetActive(false);
            SecretArea2Details.SetActive(false);
        }

        SetBackground(newBackground);
    }

    public void Respawn(string newBackground, GameObject Character, Vector2 Destination)
    {
        StopAllCoroutines();
        StartCoroutine(Teleport(newBackground,  Character, Destination));
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
        else if (currentBackground == SECRET_AREA_1)
        {
            SecretArea1.SetActive(false);
            SecretArea1Details.SetActive(false);
        }
        else if (currentBackground == SECRET_AREA_2)
        {
            SecretArea2.SetActive(false);
            SecretArea2Details.SetActive(false);
        }

        SetBackground(newBackground);
    }
}
