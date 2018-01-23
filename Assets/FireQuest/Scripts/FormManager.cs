using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormManager : MonoBehaviour {

    public InputField email;
    public InputField password;
    public Button signUpBtn;
    public Button loginBtn;
    public AuthManager authManager;
    public Text statusText;

    private void Awake()
    {
        ToggleButtonStates(false);

        // Auth delegate subscriptions
        authManager.authCallback += HandleAuthCallback;
    }

    /// <summary>
	/// Validates the email input
	/// </summary>
	public void ValidateEmail()
    {
        string emailInput = email.text;
        var regexPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        if (emailInput != "" && Regex.IsMatch(emailInput, regexPattern))
        {
            ToggleButtonStates(true);
        }
        else
        {
            ToggleButtonStates(false);
        }
    }

    public void OnSignUp()
    {
        authManager.SignUp(email.text, password.text);
        Debug.Log("Sign Up");

    }

    public void OnLogin()
    {
        authManager.LoginExistingUser(email.text, password.text);
        Debug.Log("Login");

    }

    IEnumerator HandleAuthCallback(Task<Firebase.Auth.FirebaseUser> task, string operation)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            UpdateStatus("Sorry , there was an error creating your new account. ERROR: " + task.Exception);
        }
        else if (task.IsCompleted)
        {
            Firebase.Auth.FirebaseUser newPlayer = task.Result;
            UpdateStatus("Loading the game scene");

            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Player List");
        }
    }

    void OnDestroy()
    {
        authManager.authCallback -= HandleAuthCallback;
    }

    // Utilities
    private void ToggleButtonStates(bool toState)
    {
        signUpBtn.interactable = toState;
        loginBtn.interactable = toState;
    }

    private void UpdateStatus(string message) {
        statusText.text = message;
    }
}
