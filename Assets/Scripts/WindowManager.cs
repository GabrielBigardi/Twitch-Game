using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle toggleFullscreen;

    private Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        foreach (var resolution in resolutions)
        {
            resolutionDropdown.AddOptions(new List<string> { resolution.ToString() });
        }

        Resolution curResolution = Screen.currentResolution;
        string curResolutionString = curResolution.ToString();

        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            var dropdownRes = resolutionDropdown.options[i].text;
            if(dropdownRes == curResolutionString)
            {
                resolutionDropdown.value = i;
            }
        }
    }

    public void ApplyResolution()
    {
        Resolution selectedResolution = resolutions[resolutionDropdown.value];
        bool fullscreen = toggleFullscreen.isOn;
        print("Resolução: " + selectedResolution);
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreen);
    }
}
