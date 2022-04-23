using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to display a helpful hint and a brief moment of humor if needed.
/// </summary>
public class hintDisplay : MonoBehaviour
{
    // External objects
    public GameObject g_hintText;
    public GameObject g_hintBackground;
    
    // Hint Variables
    public float hintDelay = 10.0f;
    float hintTimer = 0.0f;
    

    // Update is called once per frame
    void Update()
    {
        if (hintTimer > hintDelay)
        {
            // Script is attached to this object, to keep timer running objects are called seperately.
            g_hintText.gameObject.SetActive(true);
            g_hintBackground.gameObject.SetActive(true);
        }
        hintTimer += Time.deltaTime;
    }

    /// <summary>
    /// Called from main MCD script, stops the hint from showing after a button has been hit.
    /// </summary>
    public void disableHint()
    {
        this.gameObject.SetActive(false);
    }
}
