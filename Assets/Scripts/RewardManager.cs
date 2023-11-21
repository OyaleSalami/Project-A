using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    Text rewardDisplay;
    string reward;

    void Start()
    {
        Clear();
    }

    public void AddValue(string value)
    {
        reward += value;
        ProcessValue();
    }

    void ProcessValue()
    {
        for (int i = 0; i < reward.Length; i++)
        {
            if (i % 3 == 0)
            {
                reward.Insert(i, "'");
            }
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        rewardDisplay.text = reward;
    }

    public void Clear()
    {
        reward = "";
        ProcessValue();
    }
}
