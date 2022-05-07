using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;       // Used to recreate enumerator
using UnityEngine;


/// <summary>
/// Version 1.0 - Demo of faults only
/// Version 1.1 - Added extra functions, comm, failure pages
/// Version 2.0 - Code refactor, added 3-d collision buttons, merged button manager
/// </summary>
public class mcdManagerDemo : MonoBehaviour
{
    public enum Mode
    {
        MCD_Null,
        MSG_Clear,

        Comm_Main,
        
        Msn_Index_1,
        Msn_Index_2,
        Msn_EGT_Overtemp,
        Msn_AvionicsFaults,
        Msn_AvionicsFaults_PageUp,
        Msn_AvionicsFaults_PageDown,

        Msn_NonAvionicsFaults_1,
        Msn_NonAvionicsFaults_2,

        Msn_Maint,
        
        Quit,
        NotSim
    }

    // TODO Cleanup variables and demo variants 

    // External objects
    [Header("MCD Buttons")]
    public buttonMCD button_T1;
    public buttonMCD button_T2;

    public buttonMCD button_L1;
    public buttonMCD button_L2;
    public buttonMCD button_L3;
    public buttonMCD button_L4;
    public buttonMCD button_L5;
    public buttonMCD button_L6;

    public buttonMCD button_R1;
    public buttonMCD button_R2;
    public buttonMCD button_R3;
    public buttonMCD button_R4;
    public buttonMCD button_R5;
    public buttonMCD button_R6;

    public buttonMCD button_M1;
    public buttonMCD button_M2;
    public buttonMCD button_M3;
    public buttonMCD button_M4;
    public buttonMCD button_M5;
    public buttonMCD button_M6;


    [Header("Supporting Objects")]
    
    public MeshRenderer msg_btn_on;
    public MeshRenderer msg_btn_off;

    public formatText displayPanel;
    public formatText displayInverted;

    public systemManager globalAccess;

    public AvionicsFaultPage avionicsFaultPage;

    // Variables
    public int avionicsFaults_CurrentPage = 1;
    public int avionicsFaults_TotalPages = 1;

    public int nonAvionicsFaults_CurrentPage = 1;

    public string currentDisp;
    public string currentDispRev;

    public AudioSource sndHFchime;
    public AudioSource sndDenied;

    bool Msg_On = true;
    
    public bool MCD_ForceRedraw = true;
    public bool MCD_Is_Ready = false;

    public GameObject failScreen_Rig;
    public GameObject failScreen_Init;
    public GameObject shareware_Screen;

    public AudioSource[] snd_NO = new AudioSource[6];

    public bool failShown = false;
    

    public Mode mcdCurrentMode = Mode.Msn_Index_1;
    Mode mcdLastMode = Mode.Msn_Index_1;

    public NoteFader note;

    // Update is called once per frame
    private void Start()
    {
         
    }

    void Update()
    {
        if (failShown) return;
        if (mcdCurrentMode == mcdLastMode && MCD_ForceRedraw == false) return;  // Only update if needed

        ResetButtons();                             // If screens are changing then buttons functions will too

        if (mcdCurrentMode == Mode.Msn_Index_1) MsnIndex1();
        else if (mcdCurrentMode == Mode.Msn_Index_2) MsnIndex2();
        else if (mcdCurrentMode == Mode.Msn_EGT_Overtemp) disp_Msn_EGTOvertemp();

        else if (mcdCurrentMode == Mode.Msn_AvionicsFaults) genAvFaults(avionicsFaults_CurrentPage);
        else if (mcdCurrentMode == Mode.Msn_NonAvionicsFaults_1) Disp_Msn_NonAviFaults_1();
        else if (mcdCurrentMode == Mode.Msn_NonAvionicsFaults_2) Disp_Msn_NonAviFaults_2();

        
        // Record last mode to force screen refresh, toggled events must occur after this or screen will not redraw.
        MCD_ForceRedraw = false;        // Redraw complete, reset this no matter what
        // Check for mode toggles

        // Clear message
        if (mcdCurrentMode == Mode.MSG_Clear)
        {
            MCD_ForceRedraw = true;
            ClearNextStatus();
            mcdCurrentMode = mcdLastMode;
        }

        else if(mcdCurrentMode == Mode.Comm_Main)
        {
            failShown = true;
            shareware_Screen.SetActive(true);
            mcdCurrentMode = mcdLastMode;
        }

        else if (mcdCurrentMode == Mode.Msn_Maint)
        {
            failShown = true;
            shareware_Screen.SetActive(true);
            mcdCurrentMode = mcdLastMode;
        }
        else if (mcdCurrentMode == Mode.Msn_AvionicsFaults_PageUp)
        {
            MCD_ForceRedraw = true;
            avionicsFaults_CurrentPage++;
            if (avionicsFaults_CurrentPage > avionicsFaults_TotalPages) avionicsFaults_CurrentPage = 1;
            mcdCurrentMode = mcdLastMode;
        }

        else if (mcdCurrentMode == Mode.Msn_AvionicsFaults_PageDown)
        {
            MCD_ForceRedraw = true;
            avionicsFaults_CurrentPage--;
            if (avionicsFaults_CurrentPage < 1) avionicsFaults_CurrentPage = avionicsFaults_TotalPages;
            mcdCurrentMode = mcdLastMode;
        }

        
        mcdLastMode = mcdCurrentMode;
    }

    ///////////////////////// START DISPLAY CODE /////////////////////
   
    void MsnIndex1()
    {
        // Setup buttons
        button_M4.nextMode = (int)Mode.Msn_Index_2;
        button_M5.nextMode = (int)Mode.Msn_Index_2;
        button_M6.nextMode = (int)Mode.Msn_Index_1;

        // Build display data
        if (globalAccess.misc_IRUaligned == true)
        {
            currentDisp =
            "       msn index @1      " + "\n" +
            "                        " + "\n" +
            "<MSN INIT               " + "\n" +
            "                        " + "\n" +
            "<NAV SENSOR    APPROACH>" + "\n" +
            "                        " + "\n" +
            "               A/D MENU>" + "\n" +
            "                        " + "\n" +
            "               R/Z MENU>" + "\n" +
            "                        " + "\n" +
            "<FUEL PLAN     AOC CONV>" + "\n" +
            "                        " + "\n" +
            "               ATS MENU>";

            currentDisp += GenerateLastLine();
            currentDisp += "1/2";



            currentDispRev =
            "                        " + "\n" +
            "                        " + "\n" +
            "              TOLD MENU>" + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "<ROUTE DATA             " + "\n" +
            "                        " + "\n" +
            "<WT-CG                  " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "<GPWS/TAWS              " + "\n" +
            "                        ";
        }       // MSN Index 1 IRUs aligned
        else
        {
            currentDisp =
            "       msn index 1      " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "               APPROACH>" + "\n" +
            "                        " + "\n" +
            "               A/D MENU>" + "\n" +
            "                        " + "\n" +
            "               R/Z MENU>" + "\n" +
            "                        " + "\n" +
            "<FUEL PLAN     AOC CONV>" + "\n" +
            "                        " + "\n" +
            "               ATS MENU>";
            currentDisp += GenerateLastLine();
            currentDisp += "1/2";


            currentDispRev =
            "                        " + "\n" +
            "                        " + "\n" +
            "<MSN INIT     TOLD MENU>" + "\n" +
            "                        " + "\n" +
            "<NAV SENSOR             " + "\n" +
            "                        " + "\n" +
            "<ROUTE DATA             " + "\n" +
            "                        " + "\n" +
            "<WT-CG                  " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "<GPWS/TAWS              " + "\n" +
            "                        ";
        }

        displayPanel.Text(currentDisp);
        displayInverted.Text(currentDispRev);
    }

    void MsnIndex2()
    {
        // MSN Index 2 block 21
        // Setup buttons
        button_L1.nextMode = (int)Mode.Msn_AvionicsFaults;
        button_L2.nextMode = (int)Mode.Msn_NonAvionicsFaults_1;
        button_L4.nextMode = (int)Mode.Msn_EGT_Overtemp;
        button_L6.nextMode = (int)Mode.Msn_Maint;
        button_M4.nextMode = (int)Mode.Msn_Index_1;
        button_M5.nextMode = (int)Mode.Msn_Index_1;
        button_M6.nextMode = (int)Mode.Msn_Index_2;

        // Build display data
        currentDisp =
        "      msn index @2       " + "\n" +
        " avionic                " + "\n" +
        "<FAULT LIST   PERF FACT>" + "\n" +
        " non@-avionic            " + "\n" +
        "<FAULT LIST    MC ERASE>" + "\n" +
        " printer      permanent " + "\n" +
        "<SELECT        DATABASE>" + "\n" +
        "                 custom " + "\n" +
        "<EGT OVERTEMP  DATABASE>" + "\n" +
        " database       pos@/alt " + "\n" +
        "<LOADING    CONVERSIONS>" + "\n" +
        "                        " + "\n" +
        "<MAINTENANCE      CLEAR>";

        currentDisp += GenerateLastLine();
        currentDisp += "2/2";

        displayInverted.Text("");
        displayPanel.Text(currentDisp);

    }

    
    void disp_Msn_EGTOvertemp()     // EGT Overtemp
    {
        // Setup buttons
        button_R6.nextMode = (int)Mode.Msn_Index_2;
        button_M6.nextMode = (int)Mode.Msn_Index_2;

        // Build display data
        currentDisp =
            "     egt overtemps      \n" +
            "        engine @#        \n" +
            "  1     2     3     4   \n" +
            "           tod          \n" +
            "2245z ----z ----z ----z \n" +
            "         max egt        \n" +
            " 660  ----  ----  ----  \n" +
            "         duration       \n" +
            "00:27 --:-- --:-- --:-- \n" +
            "          region        \n" +
            "  a     -     -     -   \n" +
            "                        \n" +
            "                 return>";

        currentDisp += GenerateLastLine();

        displayInverted.Text("");
        displayPanel.Text(currentDisp);
    }


    /// <summary>
    /// Generates avionics fault list based on current faults
    /// </summary>
    /// <param name="selectedPage">Selects the current fault page for avionics faults</param>
    void genAvFaults(int selectedPage)
    {

        // Setup buttons
        button_R6.nextMode = (int)Mode.Msn_Index_2;
        button_M4.nextMode = (int)Mode.Msn_AvionicsFaults_PageUp;
        button_M5.nextMode = (int)Mode.Msn_AvionicsFaults_PageDown;
        button_M6.nextMode = (int)Mode.Msn_Index_2;

        // Build display data
        avionicsFaultPage.UpdatePages();                // *** Holy crap will this be inefficient, should only be called if faults have been modified at runtime only used for debug testing. But here we are.***

        avionicsFaults_TotalPages = avionicsFaultPage.totalPages;

        if (selectedPage > avionicsFaultPage.totalPages)
        {
            selectedPage = 1;
            avionicsFaults_CurrentPage = avionicsFaultPage.totalPages;
        }


        int startIndex = (selectedPage - 1) * 6;
        int endIndex = ((selectedPage - 1) * 6) + 6;


        currentDisp =
        "   avionics fault list  " + "\n" +
        "                        " + "\n";

        for (int i = startIndex; i < endIndex; i++)
        {
            currentDisp += avionicsFaultPage.faults[i];


            // Status line - Has to compensate for size of fault if present, repeats for all required lines
            if (i == startIndex)
            {
                for (int j = 0; j < 17 - avionicsFaultPage.faults[i].Length; j++)
                {
                    currentDisp += " ";
                }
                currentDisp += "STATUS<";
            }

            // Ethernet line
            if (i == startIndex + 1)
            {
                for (int j = 0; j < 15 - avionicsFaultPage.faults[i].Length; j++)
                {
                    currentDisp += " ";
                }
                currentDisp += "ETHERNET<";
            }

            // Return line
            if (i == startIndex + 5)
            {
                for (int j = 0; j < 17 - avionicsFaultPage.faults[i].Length; j++)
                {
                    currentDisp += " ";
                }
                currentDisp += "return<";
            }

            if (i < endIndex - 1) currentDisp += "\n\n";
        }



        currentDisp += GenerateLastLine();
        currentDisp += selectedPage + "/" + avionicsFaultPage.totalPages;

        displayInverted.Text("");
        displayPanel.Text(currentDisp);
    }

    /// <summary>
    /// Display non-avionics faultspage 1
    /// </summary>
    void Disp_Msn_NonAviFaults_1()
    {
        // Setup buttons
        button_R6.nextMode = (int)Mode.Msn_Index_2;
        button_M4.nextMode = (int)Mode.Msn_NonAvionicsFaults_2;
        button_M5.nextMode = (int)Mode.Msn_NonAvionicsFaults_2;
        button_M6.nextMode = (int)Mode.Msn_Index_2;

        // Build display data
        currentDisp =
        " non@-avionics fault list" + "\n" +
        "                        " + "\n" +
        "APM-A1APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-A2APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-A3APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-A4APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-B1APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-B2APMRXND    return>";
        
        
        currentDisp += GenerateLastLine();
        currentDisp += "1/2";

        displayInverted.Text("");
        displayPanel.Text(currentDisp);
    }

    /// <summary>
    /// Display non-avionics faultspage 2
    /// </summary>
    void Disp_Msn_NonAviFaults_2()
    {
        // Setup buttons
        button_R6.nextMode = (int)Mode.Msn_Index_2;
        button_M4.nextMode = (int)Mode.Msn_NonAvionicsFaults_1;
        button_M5.nextMode = (int)Mode.Msn_NonAvionicsFaults_1;
        button_M6.nextMode = (int)Mode.Msn_Index_2;

        // Build display data
        if (globalAccess.fault_BATT == false)
        {
            currentDisp =
        " non@-avionics fault list" + "\n" +
        "                        " + "\n" +
        "APM-B3APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-B4APMRXND           " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";
        }
        else
        {
            currentDisp =
        " non@-avionics fault list" + "\n" +
        "                        " + "\n" +
        "APM-B3APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-B4APMRXND           " + "\n" +
        "                        " + "\n" +
        "WCC-BAT CHG 1           " + "\n" +
        "                        " + "\n" +
        "WCC-BAT 1               " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";
        }
        currentDisp += GenerateLastLine();
        currentDisp += "2/2";

        displayInverted.Text("");
        displayPanel.Text(currentDisp);
    }
    ///////////////// Status light functions ////////////////////


    void SetStatusLight()       // Sets the status light
    {
        // Update message light status, should only be done once per frame
        if (Msg_On == true)
        {
            msg_btn_off.enabled = false;
            msg_btn_on.enabled = true;
        }
        else
        {
            msg_btn_off.enabled = true;
            msg_btn_on.enabled = false;
        }
    }

    // Clears the status in a clumsy way
    void ClearNextStatus()
    {
        if (globalAccess.stat_NotSim == true)       // If not sim message is shown, ignore response.
        {
            return;
        }

        else if (globalAccess.stat_DBmismatch == true)
        {
            globalAccess.stat_DBmismatch = false;
            return;
        }
        else if (globalAccess.stat_ChMast == true)
        {
            globalAccess.stat_ChMast = false;
            return;
        }
        else if (globalAccess.stat_datalink == true)
        {
            globalAccess.stat_datalink = false;
            return;
        }

    }

    // Gets our status string, called when rendering the MCD page
    string UpdateStatus()
    {
        if (globalAccess.stat_NotSim == true)
        {
            Msg_On = true;
            SetStatusLight();
            return "NOT SIMULATED";
        }

        else if (globalAccess.stat_DBmismatch == true)
        {
            Msg_On = true;
            SetStatusLight();
            return "COMM DB MISCOMPARE";
        }
        else if (globalAccess.stat_ChMast == true)
        {
            Msg_On = true;
            SetStatusLight();
            return "CHANGE MASTER";
        }
        else if (globalAccess.stat_datalink == true)
        {
            Msg_On = true;
            SetStatusLight();
            return "NO HF DATALINK";
        }
        else
        {
            Msg_On = false;
            SetStatusLight();
            return "(              )";
        }
        
    }


    /// <summary>
    /// Generates the last line of the MCD based on scratch pad status
    /// </summary>
    /// <returns>A string with the last line, either a fault or a typing prompt</returns>
    string GenerateLastLine()
    {
        string lastLine;
        string statusMessage = UpdateStatus();
        lastLine = "\n" + statusMessage;

        for (int i = 0; i < 21 - statusMessage.Length; i++)
        {
            lastLine += " ";
        }
        return (lastLine);
    }

    /// <summary>
    /// This resets all buttons to a disabled state
    /// </summary>
    void ResetButtons()
    {
        button_T1.nextMode = (int)Mode.MSG_Clear;
        button_T2.nextMode = (int)Mode.MCD_Null;
        
        button_L1.nextMode = (int)Mode.MCD_Null;
        button_L2.nextMode = (int)Mode.MCD_Null;
        button_L3.nextMode = (int)Mode.MCD_Null;
        button_L4.nextMode = (int)Mode.MCD_Null;
        button_L5.nextMode = (int)Mode.MCD_Null;
        button_L6.nextMode = (int)Mode.MCD_Null;
        
        button_R1.nextMode = (int)Mode.MCD_Null;
        button_R2.nextMode = (int)Mode.MCD_Null;
        button_R3.nextMode = (int)Mode.MCD_Null;
        button_R4.nextMode = (int)Mode.MCD_Null;
        button_R5.nextMode = (int)Mode.MCD_Null;
        button_R6.nextMode = (int)Mode.MCD_Null;

        button_M1.nextMode = (int)Mode.Comm_Main;
        button_M2.nextMode = (int)Mode.Msn_Index_1;
        button_M3.nextMode = (int)Mode.MCD_Null;
        button_M4.nextMode = (int)Mode.MCD_Null;
        button_M5.nextMode = (int)Mode.MCD_Null;
        button_M6.nextMode = (int)Mode.MCD_Null;
    }

    public void ResetFail()
    {
        failShown = false;
    }

    public void setMode(int i)
    {
        mcdCurrentMode = (Mode)i;
    }
}
