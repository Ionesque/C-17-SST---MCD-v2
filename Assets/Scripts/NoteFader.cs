using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Designed to do one thing, flash and fade the hint text. Attach it directly to the UI item you need to empathasize.
/// </summary>
public class NoteFader : MonoBehaviour
{
    Color colorNormal = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
    Color colorFlash = new Vector4(1.0f, 0.2f, 0.2f, 1.0f);

    float timer = 1.0f;
    float maxDelay = 1.0f;

    bool isStopped = true;

    Text textObject;

    // Start is called before the first frame update
    void Start()
    {
        textObject = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopped) return;
        if (timer > maxDelay)
        {
            isStopped = true;
            textObject.color = colorNormal;
        }
        else
        {
            // NOTE: If you want to change this value to anything but one second the below will need to be altered to compensate
            textObject.color = Vector4.Lerp(colorFlash, colorNormal, timer);
            timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Flashes the color of the hint text to remind the instructor as well as the student that this is MCD #3.
    /// </summary>
    public void initFlash()
    {
        timer = 0.0f;
        isStopped = false;
    }
}
