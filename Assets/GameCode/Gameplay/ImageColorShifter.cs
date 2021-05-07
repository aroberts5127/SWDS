using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorShifter : MonoBehaviour {

    public bool isImage;
    public float colorChangeTime;

    private float colorChangeRate = .05f;
    private Color newColor;
    private bool ascend = true;

    public void Start()
    {
        StartCoroutine(ShiftColorAlongSpectrum());
    }

    public void Update()
    {
        updateColor();
    }


    private IEnumerator ShiftColorAlongSpectrum()
    {
        float r = 1;
        float b = 0;
        float g = 0;
        while (true)
        {
            newColor.r = r;
            if (ascend)
            {
                while (g < 1)
                {
                    g += colorChangeRate;
                    yield return new WaitForSeconds(colorChangeTime);
                    //Debug.Log("R: " + r + ", G: " + g + ", B: " + b);
                    newColor.g = g;
                    while (b < 1)
                    {
                        b += colorChangeRate;
                        yield return new WaitForSeconds(colorChangeTime);
                        newColor.b = b;
                        //Debug.Log("R: " + r + ", G: " + g + ", B: " + b);
                    }
                }
                ascend = false;
            }
            else
            {

                while (g > 0.05f)
                {
                    g -= colorChangeRate;
                    yield return new WaitForSeconds(colorChangeTime);
                    newColor.g = g;
                    //Debug.Log("R: " + r + ", G: " + g + ", B: " + b);
                    while (b > 0.05f)
                    {
                        b -= colorChangeRate;
                        yield return new WaitForSeconds(colorChangeTime);
                        newColor.b = b;
                        //Debug.Log("R: " + r + ", G: " + g + ", B: " + b);
                    }
                }
                ascend = true;
            }
            yield return null;
        }
    }

    private void updateColor()
    {
        if (isImage)
            this.GetComponent<Image>().CrossFadeColor(newColor, 0, false, false);
        else
            this.GetComponent<MeshRenderer>().material.color = newColor;
    }
}

