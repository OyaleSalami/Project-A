using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIGradient))]
public class Background : MonoBehaviour
{
    static UIGradient gradient;

    [SerializeField] static List<Color> color1;
    [SerializeField] static List<Color> color2;
    [SerializeField] static int colorSelected = 0;

    private void Start()
    {
        gradient = GetComponent<UIGradient>();
        color1 = new List<Color>();
        color2 = new List<Color>();

        color1.Add(new Color(81, 53, 90)); //Purple
        color2.Add(new Color(42, 12, 78)); //Darker Purple
     
        color1.Add(new Color(67, 164, 159)); //Sky Blue
        color2.Add(new Color(123, 231, 88)); //Green
       
        color1.Add(new Color(194, 141, 84)); //Brown
        color2.Add(new Color(81, 53, 90)); //Purple

        color1.Add(new Color(67, 164, 159)); //Sky Blue
        color2.Add(new Color(42, 12, 78)); //Darker Purple
    }

    static public void UpdateColor()
    {
        if (colorSelected >= color1.Count - 1)
        {
            colorSelected = 0; // Loop the colors
        }
        else
        {
            colorSelected += 1; // Move on to the next color
        }

        gradient.m_color1 = color1[colorSelected];
        gradient.m_color2 = color2[colorSelected];
    }
}