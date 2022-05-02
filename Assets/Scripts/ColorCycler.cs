using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// To be attached to fail & shareware screens for any processing needed. Currently this only cycles colors.
/// </summary>
public class ColorCycler : MonoBehaviour
{
    float timer = 0.0f;
    const float maxDelay = 0.05f;

    Color color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    public Text pressToContinue;
    Vector3 originalPosition;

    private void Start()
    {
        originalPosition = pressToContinue.transform.position;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxDelay)
        {
            UpdateColor();
            AngryShakyText();
        }
    }

    /// <summary>
    /// Cycles the color to draw more attention
    /// </summary>
    void UpdateColor()
    {
        timer = 0.0f;
        color = new Vector4(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), 1.0f);    // Generate random pastel color
        pressToContinue.color = color;
    }

    /// <summary>
    /// Shakes the text to look more angry, accentuates how important it is not to do that!
    /// </summary>
    void AngryShakyText()
    {
        pressToContinue.transform.position = originalPosition + new Vector3(Random.Range(0.0f, 5.0f), Random.Range(0.0f, 5.0f), 0.0f);
    }
}
