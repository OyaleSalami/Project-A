using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLoader : MonoBehaviour
{
    [Header("UI/UX")]
    [SerializeField] int eventIndex; //The next event to be loaded
    [SerializeField] GameObject spawnAnchor;//Spawn Transform

    [SerializeField] GameObject postPrefab; //Prefab for the event object
    [SerializeField] GameObject prevPost; //Reference to the previous object
    [SerializeField] RectTransform scrollViewContentHolder;//ScrollView Content Transform

    void Start()
    {
        //GameManager.instance.GetEventsFromCloud();
        eventIndex = 0;
    }

    public void AddNextEvents()
    {
        //Add 3 events at a time
        for (int i = 0; i < 3; i++)
        {
            if(eventIndex > GameManager.instance.events.Count)
            {
                break;
            }
            else
            {
                AddEvent(GameManager.instance.events[eventIndex + i]);
                eventIndex += 1;
            }
        }
    }

    public void AddEvent(string eventId)
    {
        CreateNewEventLine(eventId);
    }

    public void CreateNewEventLine(string id)
    {
        float newHeight = postPrefab.GetComponent<RectTransform>().rect.height + 5;

        //Create a new event and position it properly
        GameObject temp = Instantiate(postPrefab, spawnAnchor.transform);
        temp.transform.position = prevPost.transform.position - new Vector3(0, newHeight, 0);

        scrollViewContentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollViewContentHolder.rect.height + newHeight);
        
        prevPost = temp;
        temp.GetComponent<PostObject>().LoadEvent(id);
    }
}
