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

    public int nextScreen = 0;
    public int lastScreen = 0;

    bool pressed = false;

    public AudioSource s_Click;
    public AudioSource s_Denied;



    // Start is called before the first frame update
    void Start()
    {
        t = this.GetComponent<Transform>();
        upPos = t.localPosition;
        downPos = new Vector3(upPos.x, upPos.y - 0.25f, upPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (pressTimer > pressDelay)
        {
            if(nextScreen != 0)
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
        string dbgStr = "Timer:" + pressTimer + ", Next:" + nextScreen;
        Debug.Log(dbgStr);

        if (nextScreen == 0)
        {
            s_Denied.Play();
            
        }
        else if(pressTimer > pressDelay)
        {
            // Call next screen from MCD object
            s_Click.Play();
        }
        
        highlightActive.SetActive(false);
        highlightDisabled.SetActive(true);
        t.localPosition = downPos;
        pressTimer = 0.0f;
    }
}
