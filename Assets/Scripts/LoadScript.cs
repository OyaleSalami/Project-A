using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour
{
    public static float timer;
    public static string scene;
   
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

    public static void LoadScene(float t, string name)
    {
        Reset();
        timer = t;
        scene = name;
        SceneManager.LoadScene("Load", LoadSceneMode.Single);
    }

    public static void Reset()
    {
        timer = 0f;
        scene = "";
    }
}
