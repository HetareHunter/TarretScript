using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TarretScreenSliderChanger : MonoBehaviour, IChangeSightColor
{
    [SerializeField] GameObject[] fillBases;
    Image[] fillBaseImgs;
    [SerializeField] Color blue;
    [SerializeField] Color red;

    // Start is called before the first frame update
    void Start()
    {
        fillBaseImgs = new Image[fillBases.Length];
        for (int i = 0; i < fillBases.Length; i++)
        {
            fillBaseImgs[i] = fillBases[i].GetComponent<Image>();
        }
    }

    public void ChangeSliderFillRed()
    {
        for (int i = 0; i < fillBaseImgs.Length; i++)
        {
             fillBaseImgs[i].color = red;
        }
    }

    public void ChangeSliderFillBase()
    {
        for (int i = 0; i < fillBaseImgs.Length; i++)
        {
            fillBaseImgs[i].color = blue;
        }
    }
}
