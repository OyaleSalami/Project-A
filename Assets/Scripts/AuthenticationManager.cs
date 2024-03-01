using Firebase.Auth;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Login Inputs")]
    /// <summary>The Email for Log-In</summary>
    [SerializeField] InputField email;

    /// <summary>The Password for Log-In</summary>
    [SerializeField] InputField password;

    [Header("Sign Up Inputs")]
    /// <summary>The Email for Sign-Up</summary>
    [SerializeField] InputField email2;

    /// <summary>The Password for Sign-Up</summary>
    [SerializeField] InputField password2;

    /// <summary>The Password(Confirm) for Sign-Up</summary>
    [SerializeField] InputField confirmPassword;

    [Header("Error & Feedback")]
    /// <summary>The text that displays the debug statements on the screen</summary>
    [SerializeField] Text feedback;
    [SerializeField] GameObject successPanel;

    void Start()
    {
        if (GameManager.instance.isFirebaseReady == false) //Check if firebase is initialiazed
        {
            if (GameManager.instance.Initialize() != true) //Try to initialize it
            {
                throw new Exception("Unable to initialize the Game Manager");
            }
        }
    }

    /// <summary> Checks the Login inputs to make sure they are valid</summary>
    public bool CheckLoginInputs()
    {
        if (email.text == "" || email.text == null)
        {
            feedback.text = "Fill in the email field!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        if (password.text == "" || password == null)
        {
            feedback.text = "Fill in the password field!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        return true; // True == Valid
    }

    ///<returns>True if the input is valid</returns>
    public bool CheckRegisterInputs()
    {
        if (email2.text == "" || email2.text == null)
        {
            feedback.text = "Enter a valid email";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        if (password2.text == "" || password2.text == null)
        {
            feedback.text = "Fill the password field!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        if (password2.text != confirmPassword.text)
        {
            feedback.text = "The passwords don't match!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        return true; // True == Valid
    }

    /// <summary>Invokes the forgot password handler and sends a reset email</summary>
    public void ForgotPassword()
    {
        if (email.text == "" || email.text == null)
        {
            feedback.text = "Enter a valid email!";
            Invoke(nameof(ClearFeedback), 3f);
            return;
        }

        GameManager.instance.auth.SendPasswordResetEmailAsync(email.text).ContinueWith(task=>
        {
            if (task.IsCanceled)
            {
                feedback.text = "Forgot Password Operation was canceled.";
                Invoke(nameof(ClearFeedback), 3f);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                feedback.text = "Forgot Password Operation encountered an error.";
                Invoke(nameof(ClearFeedback), 3f);
                return;
            }

            feedback.text = "Password Reset email has been sent.";
            Invoke(nameof(Reload), 4f); //Reload the scene
        });
    }

    /// <summary>Login with a proper email and password</summary>
    public void Login()
    {
        if (CheckLoginInputs() == false) 
        { 
            return; 
        }

        GameManager.instance.auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                feedback.text = "Login Canceled!";
                Invoke(nameof(ClearFeedback), 4f);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                feedback.text = "Sign In Encountered a problem";
                Invoke(nameof(ClearFeedback), 4f);
                return;
            }

            AuthResult result = task.Result;
            Debug.Log("User logged in successfully: " + result.User.DisplayName + " : " + result.User.UserId);

            successPanel.SetActive(true);
            Invoke(nameof(GoToHome), 3f); //Go to home if the sign in is succesful
        });
    }

    /// <summary>Sign Up for a new account</summary>
    public void SignUp()
    {
        if (CheckRegisterInputs() == false)
        { 
            return;
        }

        GameManager.instance.auth.CreateUserWithEmailAndPasswordAsync(email2.text, password2.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                feedback.text = "Sign-Up Canceled!";
                Invoke(nameof(ClearFeedback), 4f);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                feedback.text = "Sign-Up Operation ran into an error";
                Invoke(nameof(ClearFeedback), 4f);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User Created {0} : {1}", result.User.DisplayName, result.User.UserId);

            successPanel.SetActive(true);
            Invoke(nameof(GoToProfile), 4f); //Go to the profile scene
        });
    }

    /// <summary>Go to the main scene</summary>
    public void GoToHome()
    {
        LoadScript.LoadScene(1f, "Main");
    }

    /// <summary>Go to the profile scene</summary>
    public void GoToProfile()
    {
        LoadScript.LoadScene(1f, "Profile");
    }

    /// <summary>Reloads the scene</summary>
    public void Reload()
    {
        LoadScript.LoadScene(1f, "Auth");
    }

    /// <summary>Clears the error display text</summary>
    public void ClearFeedback()
    {
        feedback.text = "";
    }

}
