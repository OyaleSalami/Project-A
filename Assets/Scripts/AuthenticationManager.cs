using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Login Inputs")]
    [SerializeField] InputField email;
    [SerializeField] InputField password;

    [Header("Sign Up Inputs")]
    [SerializeField] InputField email2;
    [SerializeField] InputField password2;
    [SerializeField] InputField confirmPassword;

    [Header("Error & Feedback")]
    [SerializeField] Text feedback;
    [SerializeField] GameObject successPanel;

    void Start()
    {
        if(GameManager.instance.isFirebaseReady == true)
        {
        }
    }

    ///<returns>True if the input is valid</returns>
    public bool CheckLoginInputs()
    {
        if(email.text == "" || email.text == null)
        {
            feedback.text = "Fill in the email field!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        if(password.text == "" || password == null)
        {
            feedback.text = "Fill in the password field!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        return true;
    }

    ///<returns>True if the input is valid</returns>
    public bool CheckRegisterInputs()
    {
        if(email2.text == "" || email2.text == null)
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
        if(password2.text != confirmPassword.text)
        {
            feedback.text = "The passwords don't match!";
            Invoke(nameof(ClearFeedback), 3f);
            return false;
        }
        return true;
    }

    public void ForgotPassword()
    {
        if (email.text == "" || email.text == null)
        {
            feedback.text = "Enter a valid email!";
            Invoke(nameof(ClearFeedback), 3f);
            return;
        }

        if (GameManager.instance.user != null)
        {
            GameManager.instance.auth.SendPasswordResetEmailAsync(email.text).ContinueWith(
                task => {
                if (task.IsCanceled)
                {
                    feedback.text = "Forgot Password was canceled.";
                    Invoke(nameof(ClearFeedback), 3f);
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    feedback.text = "Forgot Password encountered an error.";
                    Invoke(nameof(ClearFeedback), 3f);
                    return;
                }

                feedback.text = "Password Reset email has been sent.";
                Debug.Log("Password reset email sent successfully.");
                Invoke(nameof(Clear), 4f);
            });
        }
    }

    public void Login()
    {
        if (CheckLoginInputs() == false) { return; }

        GameManager.instance.auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
            task => 
            {
                if (task.IsCanceled)
                {
                    feedback.text = "Login Cancelled!";
                    Invoke(nameof(ClearFeedback), 4f);
                    return;
                }
                if(task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    feedback.text = "Sign In Encountered a problem";
                    Invoke(nameof(ClearFeedback), 4f);
                    return;
                }

                AuthResult result = task.Result;
                Debug.Log("User signed in successfully: " + result.User.DisplayName + " : " + result.User.UserId);

                successPanel.SetActive(true);
                Invoke(nameof(GoToProfile), 3f);
            });
    }

    public void SignUp()
    {
        if(CheckRegisterInputs() == false) { return; }

        GameManager.instance.auth.CreateUserWithEmailAndPasswordAsync(email2.text, password2.text).ContinueWith(
            task =>
            {
                if(task.IsCanceled)
                {
                    Debug.LogError("Login Cancelled");
                }
                if(task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                AuthResult result = task.Result;
                Debug.LogFormat("User Created {0} : {1}", result.User.DisplayName, result.User.UserId);

                successPanel.SetActive(true);
                Invoke(nameof(GoToProfile), 3f);
            });
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void GoToProfile()
    {
        SceneManager.LoadScene("Profile", LoadSceneMode.Single);
    }

    public void Clear()
    {
        SceneManager.LoadScene("Auth");
    }

    public void ClearFeedback()
    {
        feedback.text = "";
    }

}
