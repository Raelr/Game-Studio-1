using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlternativeArchitecture;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : InitialisedEntity
{

    struct PlayerSettings
    {
        public float volume;
        public int quality_index;
        public int resolutionIndex;
        public bool fullScreen;
    }

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

    [Header("Fullscreen Toggle")]
    [SerializeField]
    Toggle fullscreenToggle;

    [Header("QualityDropdown")]
    [SerializeField]
    TMP_Dropdown qualityDropDown;

    [Header("Volume Slider")]
    [SerializeField]
    Slider volumeSlider;

    Resolution[] resolutions;

    PlayerSettings currentSettings;

    public override void Initialise()
    {

        base.Initialise();

        GlobalMethods.Hide(LosePanel.gameObject);

        ProcessAllResolutions();

        LoadPlayerPrefs();

        LoadSettings();

    }

    void ProcessAllResolutions()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionStrings = new List<string>();

        int currentResolutionIndex = PlayerPrefs.HasKey("resolution") ? PlayerPrefs.GetInt("resolution") : 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate;

            resolutionStrings.Add(option);

            if (!PlayerPrefs.HasKey("resolution") && resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionStrings);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            SetVolume(currentSettings.volume);
        }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            ToggleFullscreen(currentSettings.fullScreen);
        }

        if (PlayerPrefs.HasKey("resolution"))
        {
            SetResolution(currentSettings.resolutionIndex);
        }

        if (PlayerPrefs.HasKey("quality"))
        {
            SetQuality(currentSettings.quality_index);
        }
    }

    void LoadPlayerPrefs()
    {
        currentSettings = new PlayerSettings
        {
            volume = PlayerPrefs.HasKey("volume") ? PlayerPrefs.GetFloat("volume") : 0.0f,
            quality_index = PlayerPrefs.HasKey("quality") ? PlayerPrefs.GetInt("quality") : 100,
            resolutionIndex = PlayerPrefs.HasKey("resolution") ? PlayerPrefs.GetInt("resolution") : 100,
            fullScreen = !PlayerPrefs.HasKey("fullscreen") || PlayerPrefs.GetInt("fullscreen") != 0
        };
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);

        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void ShowMainMenu()
    {
        GlobalMethods.Show(MainMenuPanel.gameObject);
    }

    public void LoadLoseScreen()
    {

        GlobalMethods.Show(LosePanel.gameObject);
    }

    public void RestartLevel()
    {

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

        volumeSlider.value = volume;

        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        qualityDropDown.value = qualityIndex;

        qualityDropDown.RefreshShownValue();

        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void ToggleFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        fullscreenToggle.isOn = isFullScreen;

        PlayerPrefs.SetInt("fullscreen", isFullScreen == false ? 0 : 1);
    }
}
