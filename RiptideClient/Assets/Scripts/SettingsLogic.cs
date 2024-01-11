using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{
    public GameObject ResolutionObject;
    public GameObject FullscreenObject;

    public int screenWidth;
    public int screenHeight;
    public bool isFullscreen;

    public void ApplySettings()
    {
        isFullscreen = FullscreenObject.GetComponent<Toggle>().isOn;
        switch (ResolutionObject.GetComponent<TMP_Dropdown>().value)
        {
            case 0: screenWidth = 3840; screenHeight = 2160; break;
            case 1: screenWidth = 3440; screenHeight = 1440; break;
            case 2: screenWidth = 2560; screenHeight = 1600; break;
            case 3: screenWidth = 2560; screenHeight = 1440; break;
            case 4: screenWidth = 2560; screenHeight = 1080; break;
            case 5: screenWidth = 2048; screenHeight = 1536; break;
            case 6: screenWidth = 2048; screenHeight = 1152; break;
            case 7: screenWidth = 1920; screenHeight = 1200; break;
            case 8: screenWidth = 1920; screenHeight = 1080; break;
            case 9: screenWidth = 1680; screenHeight = 1050; break;
            case 10: screenWidth = 1600; screenHeight = 1200; break;
            case 11: screenWidth = 1600; screenHeight = 900; break;
            case 12: screenWidth = 1536; screenHeight = 864; break;
            case 13: screenWidth = 1440; screenHeight = 900; break;
            case 14: screenWidth = 1366; screenHeight = 768; break;
            case 15: screenWidth = 1280; screenHeight = 1024; break;
            case 16: screenWidth = 1280; screenHeight = 800; break;
            case 17: screenWidth = 1280; screenHeight = 720; break;
            case 18: screenWidth = 1024; screenHeight = 768; break;
            case 19: screenWidth = 800; screenHeight = 600; break;
            case 20: screenWidth = 640; screenHeight = 360; break;
        }
        Screen.SetResolution(screenWidth, screenHeight, isFullscreen);
    }

}
