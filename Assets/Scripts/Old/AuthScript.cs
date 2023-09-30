using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;

public class AuthScript : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase Settings")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    //Login variables
    [Space]
    [Header("Login Details")]
    [SerializeField] private Text email;
    [SerializeField] private Text password;

    //Registration variables
    [Space]
    [Header("Registration Details")]
    [SerializeField] private Text regName;
    [SerializeField] private Text regEmail;
    [SerializeField] private Text regPass;
    [SerializeField] private Text regPassCheck;


    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //app = Firebase.FirebaseApp.DefaultInstance;
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = (user != auth.CurrentUser) && (auth.CurrentUser != null);

            if(!signedIn && user != null)
            {
                Debug.Log("Signed out!" + user.UserId);

                user = auth.CurrentUser;
            }

            if(signedIn == true)
            {
                Debug.Log("Signed in!" + user.UserId);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(email.text, password.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException exception = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)exception.ErrorCode;

            string failMessage = "Login failed because: ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failMessage += "Email Is Invalid!";
                    break;
                case AuthError.WrongPassword:
                    failMessage += "Wrong Password!";
                    break;
                case AuthError.MissingEmail:
                    failMessage += "Email Is Missing!";
                    break;
                case AuthError.MissingPassword:
                    failMessage += "Password Is Missing!";
                    break;
                default:
                    failMessage = "Login Failed!";
                    break;
            }
            Debug.Log(failMessage);
        }
        else
        {
            user = loginTask.Result.User;

            Debug.Log("You are successfully logged in: " + user.DisplayName);
        }
    }
}
