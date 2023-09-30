using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using Mumble;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EventLoader : MonoBehaviour
{
    [Header("Events Listings")]
    List<string> eventList;

    [Header("UI/UX")]
    ///<summary>The line we are on</summary>
    [SerializeField] int lineIndex;
    ///<summary>The list of all event lines</summary>
    [SerializeField] List<GameObject> eventLines; 

    [SerializeField] GameObject eventLinePrefab; //Event Line Prefab
    [SerializeField] GameObject scrollViewContent;//Spawn Transform
    [SerializeField] InfoDisplay infoDisplay;
    [SerializeField] RectTransform scrollViewContentHolder;//Content Transform

    FirebaseAuth auth;
    FirebaseUser user;
    StorageReference storageReference;
    DatabaseReference databaseReference;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Verbose;
                InitializeFirebase();
                Invoke(nameof(GetEventsFromCloud), 3f);
            }
            else
            {
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus.ToString()));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        //Initialize Database Reference
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //Initialize Storage Reference
        storageReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://mumble-ccd73.appspot.com");
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
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

    bool CheckUserProfile()
    {
        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log(user.DisplayName + " : " + user.PhoneNumber);
            return true;
        }
        else
        {
            //Goto Login
            infoDisplay.DisplayError("You have to login first", 3f);
            Invoke(nameof(GoToLogin), 5f);
            return false;
        }
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Authentication", LoadSceneMode.Single);
    }

    public void GetEventsFromCloud()
    {
        FirebaseDatabase.DefaultInstance.GetReference("event_list").GetValueAsync()
            .ContinueWithOnMainThread(task => 
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    // Do something with snapshot...
                    eventList = (List<string>)snapshot.Value;
                    Debug.Log(eventList);
                }
            });
    }


    #region UI/UX
    void AddEvent()
    {
        foreach (var id in DatabaseManager.activeEvents)
        {
            if (eventLines[lineIndex].GetComponent<EventLine>().index >= 2)
            {
                //Event Line is full, Create a new one and continue adding events
                CreateNewEventLine();
                lineIndex++;
                eventLines[lineIndex].GetComponent<EventLine>().AddEvent(id);
            }
            else
            {
                //Event Line still has space, add more events
                eventLines[lineIndex].GetComponent<EventLine>().AddEvent(id);
            }
        }
    }

    public void CreateNewEventLine()
    {
        //Create a new event line and position it properly
        GameObject temp = Instantiate(eventLinePrefab, scrollViewContent.transform);
        temp.transform.position = eventLines[eventLines.Count - 1].transform.position - new Vector3(0, 430, 0);

        scrollViewContentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollViewContentHolder.rect.height + 430);
        eventLines.Add(temp);
    }
    #endregion
}
