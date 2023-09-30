using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FirestoreManager : MonoBehaviour
{
    public Text textUI;
    public Button clickButton;

    FirebaseFirestore db;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        clickButton.onClick.AddListener(OnHandleClick);
    }


    void OnHandleClick()
    {
        int oldCount = int.Parse(textUI.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
