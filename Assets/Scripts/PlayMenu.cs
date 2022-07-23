using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    private const float MAXIMUM_PROGRESS = 100f;
    private const string PLAYERPREF_KEY_LEVEL = "level";

    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject SettingsButton;
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject LoadGameButton;
    [SerializeField] GameObject NewGameButton;
    [SerializeField] GameObject BackButton;
    [SerializeField] GameObject NoSavedData;
    [SerializeField] GameObject OverwriteData;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Slider slider;
    [SerializeField] Text progressText;

    [SerializeField] GameObject MainMenuButtons;
    [SerializeField] GameObject PlayMenuButtons;

    private AudioSource buttonClick;

    private void Start()
    {
        buttonClick = StartButton.GetComponent<AudioSource>();
    }

    public void CloseSavedData()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        NoSavedData.SetActive(false);
    }

    public void DoNotOverwriteData()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        OverwriteData.SetActive(false);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1); // Proceed to Intro Level scene

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Round(Mathf.Clamp01(operation.progress / 0.9f));

            slider.value = progress;
            progressText.text = progress * MAXIMUM_PROGRESS + "%";

            yield return null;
        }
    }

    public void Play()
    {
        PlayButtonClickSound();
        PlayMenuButtons.SetActive(true);
        MainMenuButtons.SetActive(false);
        LoadGameButton.GetComponent<ButtonHover>().HoverOff();
        NewGameButton.GetComponent<ButtonHover>().HoverOff();
        BackButton.GetComponent<ButtonHover>().HoverOff();
    }

    public void LoadGame()
    {
        PlayButtonClickSound();
        if (!PlayerPrefs.HasKey(PLAYERPREF_KEY_LEVEL))
        {
            NoSavedData.SetActive(true);
            return;
        } 
        Data.LoadGame();
        PlayGame();
    }

    public void NewGame()
    {
        PlayButtonClickSound();
        if (PlayerPrefs.HasKey(PLAYERPREF_KEY_LEVEL))
        {
            OverwriteData.SetActive(true);
            return;
        }
        Data.NewGame();
        PlayGame();
    }

    public void OverwriteExistingData()
    {
        PlayButtonClickSound();
        OverwriteData.SetActive(false);
        Data.NewGame();
        PlayGame();
    }

    public void ReturnToMainMenu()
    {
        PlayButtonClickSound();
        PlayMenuButtons.SetActive(false);
        MainMenuButtons.SetActive(true);
        StartButton.GetComponent<ButtonHover>().HoverOff();
        SettingsButton.GetComponent<ButtonHover>().HoverOff();
        QuitButton.GetComponent<ButtonHover>().HoverOff();
    }

    public void ExitGame()
    {
        PlayButtonClickSound();
        Application.Quit();
    }

    private void PlayButtonClickSound()
    {
        buttonClick.Stop();
        buttonClick.Play();
    }
}
