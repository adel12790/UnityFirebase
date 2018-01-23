using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour {

    // firebase variables
    Firebase.Auth.FirebaseAuth auth;

    // Delegates
    public delegate IEnumerator AuthCallback(Task<FirebaseUser> task, string operation);
    public event AuthCallback authCallback;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }


    public void SignUp(string email, string password) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            StartCoroutine(authCallback(task, "sign_up"));
        });
    }

    public void LoginExistingUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            StartCoroutine(authCallback(task, "login"));
        });
    }
}
