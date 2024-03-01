using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary> Singleton instance of the game manager</summary>
    public static GameManager instance;

    /// <summary>Describes if Firebase is in a useable state</summary>
    public bool isFirebaseReady = false;

    /// <summary> Authentication Details about the player </summary>
    public FirebaseAuth auth;

    /// <summary> The Signed-in User </summary>
    public FirebaseUser user;

    /// <summary> Reference to the storage bucket </summary>
    public StorageReference stReference;

    /// <summary> Reference to the the database </summary>
    public DatabaseReference dbReference; //


    //public List<string> events; //List of the event IDs

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
        Initialize();
    }

    public bool Initialize()
    {
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

        return true;
    }

    void InitializeFirebase()
    {
        //TODO: Set up App Check
        //FirebaseAppCheck.SetAppCheckProviderFactory(PlayIntegrityProviderFactory.Instance);
        user = new FirebaseUser();

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);


        //Initialize Database Reference
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        //Initialize Storage Reference
        stReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");
    }

    /// <summary> Tracks state changes of the auth object </summary>
    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);

            if (!signedIn && user != null)
            {
                Debug.Log("Signed Out");
                LoadScript.LoadScene(1, "Auth");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed In");
                LoadScript.LoadScene(1, "Main");
            }
        }
    }

    /// <summary>
    /// Handle removing subscription and reference to the Auth instance.
    /// Automatically called by a Monobehaviour after Destroy is called on it.
    /// </summary>
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
                    //events.Add(s);
                }
            }
        });
    }
}
