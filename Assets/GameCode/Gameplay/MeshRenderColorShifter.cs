using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRenderColorShifter : MonoBehaviour
{
    public float colorChangeTime;
    public MeshRenderer mr;

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
                    newColor.g = g;
                    while (b < 1)
                    {
                        b += colorChangeRate;
                        yield return new WaitForSeconds(colorChangeTime);
                        newColor.b = b;
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
                    while (b > 0.05f)
                    {
                        b -= colorChangeRate;
                        yield return new WaitForSeconds(colorChangeTime);
                        newColor.b = b;
                    }
                }
                ascend = true;
            }
            yield return null;
        }
    }

    private void updateColor()
    {
        mr.material.color = newColor;
    }
}
