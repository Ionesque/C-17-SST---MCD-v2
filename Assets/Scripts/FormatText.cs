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
    public TextMesh r_LayerFill;

    public bool inverted = false;

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

        char[] charOriginal = new char[maxStringSize];
        char[] charUpper = new char[maxStringSize];
        char[] charLower = new char[maxStringSize];
        char[] charFill = new char[maxStringSize];

        charOriginal = s.ToCharArray();
        int correctedPos = 0;


        for (int i = 0; i < charOriginal.Length; i++)       // Checking for lowercase letters in uppercase array
        {
            if(charOriginal[i] == 64)       // This accepts @ as an escape character for lower case
            {
                i++;                                                // Escape found skip to next index in array
                charLower[correctedPos] = charOriginal[i];
                charUpper[correctedPos] = (char)32;                 // No matter what charUpper needs to be initialized with a blank at all spots.
            }
            else
            {
                charUpper[correctedPos] = charOriginal[i];
            }
            
            if (inverted)
            {
                if (charUpper[correctedPos] >= 33 && charUpper[correctedPos] <= 126) charFill[correctedPos] = (char)87;
                else if (charUpper[correctedPos] == 10) charFill[correctedPos] = (char)10;    // Newline check
                else charFill[correctedPos] = (char)32;                            // Fills with "W" for best coverage. 
            }


            if (charUpper[correctedPos] >= 97 && charUpper[correctedPos] <= 122)                // Check for lowercase letter
            {
                charLower[correctedPos] = (char)(charUpper[correctedPos] - 32);               // Convert from lower to uppercase and place in lowercase array
                charUpper[correctedPos] = (char)32;                                // Remove letter from uppercase array
            }
            else if (charLower[correctedPos] >= 33 && charLower[correctedPos] <= 63) charUpper[correctedPos] = (char)32;        // if an escape character is used 
            else if (charUpper[correctedPos] == 10) charLower[correctedPos] = (char)10;       // Newline check
            else charLower[correctedPos] = (char)32;                               // Remove letter from uppercase array
            
            correctedPos++;
        }

        r_LayerUpper.text = new string(charUpper);
        r_LayerLower.text = new string(charLower);
        if (inverted) r_LayerFill.text = new string(charFill);
    }
}
    
