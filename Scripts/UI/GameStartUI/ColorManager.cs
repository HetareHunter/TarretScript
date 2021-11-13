using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIの色を変えるクラス
/// </summary>
public class ColorManager : MonoBehaviour
{
    [SerializeField] Color startColor;
    [SerializeField] Color changeColor;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ToChangeColor()
    {
        image.color = changeColor;
    }

    public void ToStartColor()
    {
        image.color = startColor;
    }
}
