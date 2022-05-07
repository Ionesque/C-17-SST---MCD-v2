using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum displayMode
{
    Off,
    Min_FromOff,
    Max,
    Min_FromMax
}

public class BrightnessKnob : MonoBehaviour
{
    // Start is called before the first frame update

    

    public Vector3[] knobPositions = new Vector3[4];

    public displayMode currentDisplayMode = displayMode.Max;

    Transform t;
    public TextMesh dispUpper;
    public TextMesh dispLower;
    public TextMesh dispInvert;
    public Material matInvertedBlocks;
    public GameObject objInvertedBlocks;
    public GameObject objInvertedUpper;
    public GameObject objInvertedLower;

    float timer = 1.0f;
    const float maxTime = 1.0f;

    bool doneMoving = false;



    void Start()
    {
        t = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doneMoving) return;
        if (timer > 1.0f)
        {
            t.localEulerAngles = knobPositions[(int)currentDisplayMode];
            doneMoving = true;
        }
        else
        {
            if (currentDisplayMode == displayMode.Off)
            {
                t.localEulerAngles = knobPositions[0];                  // Snap to off
                Color c = new Vector4(0.0f, 0.4f, 0.0f, 0.0f);
                dispUpper.color = c;
                dispLower.color = c;
                dispInvert.color = c;
                objInvertedBlocks.SetActive(false);
                objInvertedLower.SetActive(false);
                objInvertedUpper.SetActive(false);

            }
            else if (currentDisplayMode == displayMode.Min_FromOff)
            {
                t.localEulerAngles = knobPositions[1];                  // Snap to Min
                Color c = new Vector4(0.0f, 0.4f, 0.0f, 1.0f);
                objInvertedBlocks.SetActive(true);
                objInvertedLower.SetActive(true);
                objInvertedUpper.SetActive(true);
                dispUpper.color = c;
                dispLower.color = c;
                dispInvert.color = c;
                matInvertedBlocks.color = c;
                
            }
            else if (currentDisplayMode == displayMode.Max)
            {
                t.localEulerAngles = Vector3.Lerp(knobPositions[1], knobPositions[2], timer);
                Color c = Vector4.Lerp(new Vector4(0.0f, 0.4f, 0.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), timer);
                dispUpper.color = c;
                dispLower.color = c;
                dispInvert.color = c;
                matInvertedBlocks.color = c;
            }
            else if (currentDisplayMode == displayMode.Min_FromMax)
            {
                t.localEulerAngles = Vector3.Lerp(knobPositions[2], knobPositions[1], timer);
                Color c = Vector4.Lerp(new Vector4(0.0f, 1.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.4f, 0.0f, 1.0f), timer);
                dispUpper.color = c;
                dispLower.color = c;
                dispInvert.color = c;
                matInvertedBlocks.color = c;
            }
            
            timer += Time.deltaTime;
        }

    }

    private void OnMouseDown()
    {
        if (!doneMoving) return;
        if (currentDisplayMode == displayMode.Off)
        {
            currentDisplayMode = displayMode.Min_FromOff;
            timer = 0.95f;
        }
        else if (currentDisplayMode == displayMode.Min_FromOff)
        {
            currentDisplayMode = displayMode.Max;
            timer = 0.0f;
        }
        else if (currentDisplayMode == displayMode.Max)
        {
            currentDisplayMode = displayMode.Min_FromMax;
            timer = 0.0f;
        }
        else if (currentDisplayMode == displayMode.Min_FromMax)
        {
            currentDisplayMode = displayMode.Off;
            timer = 0.95f;
        }
        
        doneMoving = false;
    }
}
