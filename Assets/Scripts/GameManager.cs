using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Material mumbleTex; //Prefab for the image for the game board

    public static GameManager instance; //Singleton instance of the game manager class
    public InfoDisplay infoDisplay; //This sits on the game manager object

    public bool isFirebaseReady = false;
    public FirebaseAuth auth; //Authentication Details about the player
    public FirebaseUser user; //The Signed-in User
    public StorageReference stReference; //Reference to the storage bucket
    public DatabaseReference dbReference; //Reference to the the database 


    public List<string> events; //List of the event IDs

    private Image gameImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        infoDisplay = GetComponent<InfoDisplay>();
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
                isFirebaseReady = true;
            }
            else
            {
                isFirebaseReady = false;
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus.ToString()));
            }
        });
    }

    void InitializeFirebase()
    {
        //FirebaseAppCheck.SetAppCheckProviderFactory(PlayIntegrityProviderFactory.Instance);

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        user = new FirebaseUser();

        dbReference = FirebaseDatabase.DefaultInstance.RootReference; //Initialize Database Reference
        stReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com"); //Initialize Storage Reference
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) // Track state changes of the auth object.
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                infoDisplay.DisplayMessage("Signed out: " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                infoDisplay.DisplayMessage("Signed in: " + user.UserId);
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

    public void GetEventsFromCloud() //Get the list of events
    {
        dbReference.Child("event_list").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception); // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Do something with snapshot...
                Dictionary<string, object> tempList = (Dictionary<string, object>)snapshot.Value;
                Dictionary<string, object>.ValueCollection valCol = tempList.Values;

                foreach (string s in valCol)
                {
                    events.Add(s);
                }
            }
        });
    }

    bool CheckUserProfile()
    {
        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log(user.DisplayName + " : " + user.PhoneNumber);
            //TODO: Load profile details from here

            return true;
        }
        else
        {
            return false;
        }
    }
}
