using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMCD : MonoBehaviour
{
    const float pressDelay = 0.15f;
    public float pressTimer = pressDelay;

    systemManager SystemManager;

    public GameObject highlightActive;
    public GameObject highlightDisabled;
    public Renderer buttonRenderer;
    public Material buttonEnabled;
    public Material buttonDisabled;


    BrightnessKnob brightnessKnob;

    Transform t;
    public Vector3 upPos;
    public Vector3 downPos;

    public AudioSource s_Click;
    public AudioSource s_Denied;
    
    public int nextMode = 0;
    public bool dimsWhenDisabled = true;
    
    mcdManager m;
    mcdManagerDemo m_d;

    bool demoVersion = false;

    // Start is called before the first frame update
    void Start()
    {
        brightnessKnob = GameObject.Find("Brightness Knob").GetComponent<BrightnessKnob>();
        try
        {
            m = GameObject.Find("MCD Manager").GetComponent<mcdManager>();
        }
        catch
        {
            demoVersion = true;
            m_d = GameObject.Find("MCD Manager Demo").GetComponent<mcdManagerDemo>();

        }

        t = this.GetComponent<Transform>();
        upPos = t.localPosition;
        downPos = new Vector3(upPos.x, upPos.y - 0.25f, upPos.z);
        SystemManager = GameObject.Find("Fault Settings").GetComponent<systemManager>();
        }

    // Update is called once per frame
    void Update()
    {
        if (pressTimer > pressDelay)
        {
            if(nextMode != 0)
            {
                highlightActive.SetActive(true);
                highlightDisabled.SetActive(false);
                if (dimsWhenDisabled) buttonRenderer.material = buttonEnabled;
            }
            else
            {
                highlightActive.SetActive(false);
                highlightDisabled.SetActive(true);
                if (dimsWhenDisabled) buttonRenderer.material = buttonDisabled;
            }
            
            t.localPosition = upPos;
        }
        pressTimer += Time.deltaTime;
    }

    private void OnMouseDown()
    {
        if (demoVersion)
        {
            if (m_d.failShown) return;
        }
        else
        {
            if (m.failShown) return;
        }

        if (brightnessKnob.currentDisplayMode == displayMode.Off) return;

        if (nextMode == 0)
        {
            s_Denied.Play();
            SystemManager.notSimulated();
        }
        else if(pressTimer > pressDelay)
        {
            if(demoVersion) m_d.setMode(nextMode);        // When MCD Manager has it's value changed it will trigger a page change)
            else m.setMode(nextMode);        // When MCD Manager has it's value changed it will trigger a page change
            s_Click.Play();
            pressTimer = 0.0f;
        }
        // Induce button delay, certain touchscreens had issues registering double touches     
        highlightActive.SetActive(false);
        highlightDisabled.SetActive(true);
        t.localPosition = downPos;
        
    }

}
