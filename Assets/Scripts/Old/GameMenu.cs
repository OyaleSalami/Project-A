using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        SceneManager.LoadScene("Event Scene", LoadSceneMode.Single);
    }

    public void Win()
    {

    }

    public void Profile()
    {
        SceneManager.LoadScene("Profile Scene", LoadSceneMode.Single);
    }
}
