using System.Collections;
using UnityEngine;

public class ParallaxBackgroundManager : MonoBehaviour
{
    private const float TELEPORT_DELAY = 1.3f;
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

    private CharacterHealth characterHealth;
    private CharacterMovement characterMovement;

    public bool isTeleporting;
    private string currentBackground;

    private void Awake()
    {
        GetCharacterComponents();
    }

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
        DeactivateSecretArea2();
        currentBackground = Data.location;
        SetNewBackground(currentBackground);
        FindObjectOfType<AudioManager>().PlayMusic(currentBackground);
        isTeleporting = false;
    }

    public void SetNewBackground(string newBackground)
    {
        FindObjectOfType<AudioManager>().ChangeMusic(currentBackground, newBackground);
        currentBackground = newBackground;
        characterMovement.location = currentBackground;
        ActivateNewBackground(newBackground);
        Transition.Deactivate();
        // Return control to Character after teleport is done!!!
        if (characterHealth.IsDead())
        {
            characterHealth.Revive();
        }
        isTeleporting = false;
    }

    public IEnumerator ChangeBackground(string newBackground, GameObject character, Portal destination)
    {
        Transition.Activate();
        yield return new WaitForSeconds(TELEPORT_DELAY);
        character.transform.position = destination.transform.position;
        DeactivateBackground();
        SetNewBackground(newBackground);
    }

    public void Respawn(string newBackground, GameObject character, Vector2 destination)
    {
        StopAllCoroutines();
        StartCoroutine(Teleport(newBackground, character, destination));
    }

    public IEnumerator Teleport(string newBackground, GameObject character, Vector2 destination)
    {
        Transition.Activate();
        yield return new WaitForSeconds(TELEPORT_DELAY);
        character.transform.position = destination;
        DeactivateBackground();
        SetNewBackground(newBackground);
    }

    private void GetCharacterComponents()
    {
        characterHealth = GetComponent<CharacterHealth>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void ActivateNewBackground(string newBackground)
    {
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
    }

    private void DeactivateBackground()
    {
        if (currentBackground == INTRO)
        {
            DeactivateIntro();
        }
        else if (currentBackground == TOWN)
        {
            DeactivateTown();
        }
        else if (currentBackground == FOREST)
        {
            DeactivateForest();
        }
        else if (currentBackground == CAVE)
        {
            DeactivateCave();
        }
        else if (currentBackground == BASE_STATION)
        {
            DeactivateBaseStation();
        }
        else if (currentBackground == SPACE_STATION)
        {
            DeactivateSpaceStation();
        }
        else if (currentBackground == SECRET_AREA_1)
        {
            DeactivateSecretArea1();
        }
        else if (currentBackground == SECRET_AREA_2)
        {
            DeactivateSecretArea2();
        }
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

    private void DeactivateSecretArea2()
    {
        SecretArea2.SetActive(false);
        SecretArea2Details.SetActive(false);
    }
}
