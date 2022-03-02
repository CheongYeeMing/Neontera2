using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    private const string PLAYERPREF_KEY_LEVEL = "level";

    [SerializeField] GameObject StartMenu;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject NoSavedData;
    [SerializeField] GameObject OverwriteData;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Slider slider;
    [SerializeField] Text progressText;

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
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

    public void Play()
    {
        PlayButtonClickSound();
        StartMenu.SetActive(true);
    }

    public void LoadGame()
    {
        PlayButtonClickSound();
        if (!PlayerPrefs.HasKey("level"))
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
        if (PlayerPrefs.HasKey("level"))
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
        StartMenu.SetActive(false);
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
