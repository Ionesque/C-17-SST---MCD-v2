using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMCD : MonoBehaviour
{
    public float pressDelay = 0.5f;
    public float pressTimer = 0.5f;

    public GameObject highlightActive;
    public GameObject highlightDisabled;

    Transform t;
    public Vector3 upPos;
    public Vector3 downPos;

    public AudioSource s_Click;
    public AudioSource s_Denied;

    public Mode nextMode = Mode.MCD_Null;
    mcdManager m;
    

    // Start is called before the first frame update
    void Start()
    {
        m = GameObject.Find("MCD Manager").GetComponent<mcdManager>();
        t = this.GetComponent<Transform>();
        upPos = t.localPosition;
        downPos = new Vector3(upPos.x, upPos.y - 0.25f, upPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (pressTimer > pressDelay)
        {
            if(nextMode != Mode.MCD_Null)
            {
                highlightActive.SetActive(true);
                highlightDisabled.SetActive(false);
            }
            else
            {
                highlightActive.SetActive(false);
                highlightDisabled.SetActive(true);
            }
            
            t.localPosition = upPos;
        }
        pressTimer += Time.deltaTime;
    }

    private void OnMouseDown()
    {
        string dbgStr = "Timer:" + pressTimer + ", Next:" + nextMode;
        Debug.Log(dbgStr);

        if (nextMode == Mode.MCD_Null)
        {
            s_Denied.Play();
            
        }
        else if(pressTimer > pressDelay)
        {
            m.mcdCurrentMode = nextMode;        // When MCD Manager has it's value changed it will trigger a page change
            s_Click.Play();
        }
        // Induce button delay, certain touchscreens had issues registering double touches     
        highlightActive.SetActive(false);
        highlightDisabled.SetActive(true);
        t.localPosition = downPos;
        pressTimer = 0.0f;
    }

}
