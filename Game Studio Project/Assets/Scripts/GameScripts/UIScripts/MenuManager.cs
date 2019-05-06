using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlternativeArchitecture;
using UnityEngine.Audio;

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

    public override void Initialise() {

        base.Initialise();

        GlobalMethods.Hide(LosePanel.gameObject);
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
