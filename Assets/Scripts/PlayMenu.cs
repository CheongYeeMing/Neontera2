using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] GameObject StartMenu;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject NoSavedData;
    [SerializeField] GameObject OverwriteData;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Slider slider;
    [SerializeField] Text progressText;

    private AudioSource ButtonClick;

    private void Start()
    {
        ButtonClick = StartButton.GetComponent<AudioSource>();
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
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        StartMenu.SetActive(true);
    }

    public void LoadGame()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
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
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
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
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        OverwriteData.SetActive(false);
        Data.NewGame();
        PlayGame();
    }

    public void ReturnToMainMenu()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        StartMenu.SetActive(false);
    }

    public void ExitGame()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        Application.Quit();
    }
}
