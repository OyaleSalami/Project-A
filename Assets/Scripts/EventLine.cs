using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventLine : MonoBehaviour
{
    [SerializeField] GameObject[] eventDisplays; //Reference to the individual event displays
    [SerializeField] public int index; //Index to the current event display

    void Start()
    {
        foreach (GameObject display in eventDisplays)
        {
            display.SetActive(false);
        }
        index = 0;
    }

    public void AddEvent(string _id)
    {
        eventDisplays[index].SetActive(true);
        eventDisplays[index].GetComponent<MumbleEvent>().thisEvent.event_id = _id;
        eventDisplays[index].GetComponent<MumbleEvent>().LoadImage();
        index++;
    }
}
