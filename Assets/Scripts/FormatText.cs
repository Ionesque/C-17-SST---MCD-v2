using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to seperate upper and lowercase letters and renders to two seperate layers. Lowercase letters are short uppercase, eliminates need to create custom fonts
/// </summary>
public class formatText : MonoBehaviour
{
    [Header("Debugging Settings")]
    public bool testMode = false;
    int testChar = 33;

    [Header("Render Elements")]
    public TextMesh r_LayerUpper;
    public TextMesh r_LayerLower;

    
    const int maxStringSize = 512;                    // Display character array sizes

    

    // Update is called once per frame
    void Update()
    {
        if (testMode) characterTest();
    }

    /// <summary>
    /// Cycle through all valid characters on display
    /// </summary>
    void characterTest()
    {
        string dbgStr = "";
        int j = 0;

        for (int i = 0; i < (25 * 14); i++)
        {

            dbgStr += (char)testChar;
            j++;
            if (j >= 25)
            {
                dbgStr += "\n";
                j = 0;
            }
        }

        Text(dbgStr);

        testChar++;
        if (testChar > 126) testChar = 33;
    }


    /// <summary>
    /// Set display brightness
    /// </summary>
    /// <param name="brt">0 = dim, 255 = full</param>
    void setBrightness(int brt)
    {
        r_LayerUpper.color = new Color32(0, 255, 0, (byte)brt);
    }
    
    /// <summary>
    /// Reads a string input applies needed conversions, and renders to attached TextMeshes     
    /// </summary>
    /// <param name="s">Input string</param>
    public void Text(string s)
    {
        
        if (s.Length > maxStringSize) return;       // If for some reason the string is longer than the character array abort

        char[] charUpper = new char[maxStringSize];
        char[] charLower = new char[maxStringSize];
        
        charUpper = s.ToCharArray();
        

        for(int i = 0; i < charUpper.Length; i++)       // Checking for lowercase letters in uppercase array
        {
            if(charUpper[i] >= 97 && charUpper[i]<= 122)    // Check for lowercase letter
            {
                charLower[i] = (char)(charUpper[i] - 32);   // Convert from lower to uppercase and place in lowercase array
                charUpper[i] = (char)32;                    // Remove letter from uppercase array
            }
            else if (charUpper[i] == 10)                    // Check for new line and transfer to lowercase character array
            {
                charLower[i] = (char)10;
            }
            else
            {
                charLower[i] = (char)32;                    // Remove letter from uppercase array
            }

        }

        r_LayerUpper.text = new string(charUpper);
        r_LayerLower.text = new string(charLower);
        Debug.Log("Done Updating");
    }
}
