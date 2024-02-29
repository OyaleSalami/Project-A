using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] float timer;

    private void OnEnable()
    {
        curr_time = 0;
    }

    float curr_time = 0;
    void Update()
    {
        curr_time += Time.deltaTime;

        if (curr_time >= timer)
        {
            Disable();
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
