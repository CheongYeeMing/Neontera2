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

    private float target;

    public void Close()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        NoSavedData.SetActive(false);
    }

    public void No()
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

    public void Yes()
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

    public void CloseGame()
    {
        StartButton.GetComponent<AudioSource>().Stop();
        StartButton.GetComponent<AudioSource>().Play();
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //slider.value = Mathf.MoveTowards(slider.value, target, 3 * Time.deltaTime);
    }
}
