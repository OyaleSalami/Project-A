using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(UploadManager))]
public class UploadController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputField descInput;

    [Header("Display Image")]
    [SerializeField] Image displayImage;
    [SerializeField] GameObject imageText;
    [SerializeField] GameObject imagePlaceholder;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GoToHome()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
