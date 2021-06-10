using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField ConfirmPasswordInput;
    public Button RegisterButton;
    // Start is called before the first frame update
    void Start()
    {
        RegisterButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.LoginManager.RegisterUser(UsernameInput.text, PasswordInput.text, ConfirmPasswordInput.text));
        });
    }
}
