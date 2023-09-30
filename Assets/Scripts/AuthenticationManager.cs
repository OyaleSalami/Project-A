using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Extensions;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject phone;
    [SerializeField] GameObject otp;
    [SerializeField] GameObject successPanel;

    [Header("Inputs")]
    [SerializeField] Text phoneInput;
    [SerializeField] Text otpInput;

    [Header("Errors")]
    [SerializeField] GameObject errorText;
    [SerializeField] InfoDisplay infoDisplay;

    FirebaseAuth auth;
    FirebaseUser user;
    PhoneAuthProvider provider;

    string phoneNumber;
    string verId;
    [SerializeField] int timeOut = 200; //200 sceonds

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Verbose;
                InitializeFirebase();
            }
            else
            {
                infoDisplay.DisplayError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus.ToString()));
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus.ToString()));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out: " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in: " + user.UserId);
            }
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    // Automatically called by a Monobehaviour after Destroy is called on it.
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void SubmitPhone()
    {
        //Set the phone number
        phoneNumber = FormatPhoneNumber(phoneInput.GetComponent<Text>().text);
        if (phoneNumber.Length != 14)
        {
            errorText.SetActive(true);
            errorText.GetComponent<Text>().text = "Invalid Phone Number";
            infoDisplay.DisplayError("Invalid Phone Number!");
            return;
        }

        phone.SetActive(false);
        provider = PhoneAuthProvider.GetInstance(auth);

        provider.VerifyPhoneNumber(
          new PhoneAuthOptions
          {
              PhoneNumber = phoneNumber,
              TimeoutInMilliseconds = (uint)timeOut * 1000,
              ForceResendingToken = null
          },
          verificationCompleted: (credential) =>
          {
              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
              infoDisplay.DisplayMessage("Successfully Signed In");
              successPanel.SetActive(true);
          },
          verificationFailed: (error) =>
          {
              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
              infoDisplay.DisplayError("Error: " + error);
          },
          codeSent: (id, token) =>
          {
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.
              otp.SetActive(true);
              verId = id;
          },
          codeAutoRetrievalTimeOut: (id) =>
          {
              // Called when the auto-sms-retrieval has timed out, based on the given
              // timeout parameter.
              // `id` contains the verification id of the request that timed out.
              otp.SetActive(true);
              verId = id;
          });
    }

    public void SubmitOTP(string otp)
    {
        PhoneAuthCredential cred = provider.GetCredential(verId, otp);

        auth.SignInAndRetrieveDataWithCredentialAsync(cred).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                infoDisplay.DisplayMessage("SignInAndRetrieveDataWithCredentialAsync encountered an error: " +
                               task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            infoDisplay.DisplayMessage("User signed in successfully");

            successPanel.SetActive(true);

            // This should display the phone number.
            Debug.Log("Phone number: " + newUser.PhoneNumber);

            // The phone number providerID is 'phone'.
            Debug.Log("Phone provider ID: " + newUser.ProviderId);
        });
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Clear()
    {
        SceneManager.LoadScene("Authentication");
    }

    string FormatPhoneNumber(string str)
    {
        string phone = "+234";

        for (int i = 1; i < str.Length; i++)
        {
            phone += str[i];
        }

        return phone;
    }
}
