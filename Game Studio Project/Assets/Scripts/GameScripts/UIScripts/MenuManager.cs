using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlternativeArchitecture;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : InitialisedEntity {

    [Header("Main Audio")]
    [SerializeField]
    AudioMixer masterMixer;

    [Header("Main Menu")]
    [SerializeField]
    Image MainMenuPanel;

    [Header("Lose Screen")]
    [SerializeField]
    Image LosePanel;

    [Header("Resolution Dropdown")]
    [SerializeField]
    TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public override void Initialise() {

        base.Initialise();

        GlobalMethods.Hide(LosePanel.gameObject);

        ProcessAllResolutions();
    }

    void ProcessAllResolutions()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionStrings = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionStrings.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;

            }
        }

        resolutionDropdown.AddOptions(resolutionStrings);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void QuitGame() {

        Application.Quit();
    }

    public void ShowMainMenu()
    {
        GlobalMethods.Show(MainMenuPanel.gameObject);
    } 

    public void LoadLoseScreen() {

        GlobalMethods.Show(LosePanel.gameObject);
    }

    public void RestartLevel() {

        PlayerPrefs.SetInt("Reset", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("Reset", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetVolume(float volume)
    {
        masterMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ToggleFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
