using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] float timer = 2f; //Fade-out timer
    [SerializeField] Text infoText;
    [SerializeField] Text titleText;
    [SerializeField] GameObject displayImage;

    float curr_time = 0;
    private void OnEnable()
    {
        curr_time = 0;
    }

    void Update()
    {
        curr_time += Time.deltaTime;
        if (curr_time >= timer)
        {
            gameObject.SetActive(false);
        }
    }

    public void DisplayError(string err, float timeout = 2f)
    {
        timer = timeout;
        infoText.text = err;
        titleText.text = "ERROR";
        gameObject.SetActive(true);
        displayImage.SetActive(false);
    }

    public void DisplayMessage(string mes, float timeout = 2f)
    {
        timer = timeout;
        infoText.text = mes;
        titleText.text = "MESSAGE";
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        infoText.text = "";
        titleText.text = "";
    }
}
