using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlternativeArchitecture;
using UnityEngine.Audio;
using TMPro;
using System;

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
    AudioMixer masterMixer = null;

    [Header("Main Menu")]
    [SerializeField]
    Image MainMenuPanel = null;

    [Header("Lose Screen")]
    [SerializeField]
    Image LosePanel = null;

    [Header("Resolution Dropdown")]
    [SerializeField]
    TMP_Dropdown resolutionDropdown = null;

    [Header("Fullscreen Toggle")]
    [SerializeField]
    Toggle fullscreenToggle = null;

    [Header("QualityDropdown")]
    [SerializeField]
    TMP_Dropdown qualityDropDown = null;

    [Header("Volume Slider")]
    [SerializeField]
    Slider volumeSlider = null;

    [Header("Loading Panel")]
    [SerializeField]
    Image fadeIn = null;

    [Header("Start Options")]
    [SerializeField]
    Image optionsPanel = null;

    [Header("Options Menu")]
    [SerializeField]
    Image optionMenu = null;

    [Header("Loading panel Colors")]
    [SerializeField]
    Color loaded = Color.white;

    [SerializeField]
    Color loading = Color.white;

    [SerializeField]
    TextMeshProUGUI newRecord;

    [SerializeField]
    CountDownUI countDown;

    Coroutine loadingRoutine;

    Coroutine fadeOnceRoutine;

    Coroutine optionsFadeRoutine;

    Resolution[] resolutions;

    PlayerSettings currentSettings;

    public delegate void OnGameReset();

    public event OnGameReset onReset;

    bool isFading = false;

    bool optionsDisplayed = false;

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
            StartCoroutine(WaitForFrame());
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
        currentSettings = new PlayerSettings {
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
        Cursor.visible = true;
    }

    public void HideMainMenu()
    {
        GlobalMethods.Hide(MainMenuPanel.gameObject);
        Cursor.visible = false;
    }

    public void LoadLoseScreen(bool enabled = false)
    {
        newRecord.gameObject.SetActive(enabled);
        Cursor.visible = true;
        GlobalMethods.Show(LosePanel.gameObject);
    }

    public void HideLoseScreen() {
        Cursor.visible = false;
        GlobalMethods.Hide(LosePanel.gameObject);
    }

    public void HideOptionsMenu() {
        GlobalMethods.Hide(optionMenu.gameObject);
    }

    public void ShowMenu() {

        ShowMainMenu();

        HideOptionsMenu();

        if (optionsPanel.IsActive()) {
            FadeOutOptions();
        }
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt("Reset", 0);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetGame()
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

    public void ResetScores()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("MainScene");
    }

    public void StartGame()
    {
        if (!isFading)
        {
            RunLoadSequence(null, GameMaster.instance.StartGame, true);
        }
    }

    public void RunLoadSequence(Action endAction, Action middleAction = null, bool fadeInto = false)
    {
        if (loadingRoutine != null)
        {
            StopCoroutine(loadingRoutine);
            loadingRoutine = StartCoroutine(FadeInAndOut(endAction, middleAction, fadeInto));
        }
         
        loadingRoutine = StartCoroutine(FadeInAndOut(endAction, middleAction, fadeInto));
    }

    public void ResetAfterFade()
    {
        if (!isFading)
        {
            onReset?.Invoke();
            RunLoadSequence(null, ResetGame);
        }
    }

    public void RestartAfterFade()
    {
        if (!isFading)
        {
            onReset?.Invoke();
            RunLoadSequence(null, RestartLevel, true);
        }
    }

    public void FadeInOptionsMenu() {

        if (!optionsDisplayed)
        {
            if (!isFading)
            {
                CanvasGroup group = optionsPanel.GetComponent<CanvasGroup>();
                optionsPanel.gameObject.SetActive(true);
                float desiredAlpha = 1f;
                group.alpha = 0f;

                if (optionsFadeRoutine != null)
                {
                    StopCoroutine(optionsFadeRoutine);
                }

                optionsFadeRoutine = StartCoroutine(FadeInPanel(optionsPanel, desiredAlpha, group));
            }
        }
    }

    public void FadeOutOptions() {

        if (optionsDisplayed)
        {
            if (!isFading)
            {
                CanvasGroup group = optionsPanel.GetComponent<CanvasGroup>();
                float desiredAlpha = 0f;

                if (optionsFadeRoutine != null)
                {
                    StopCoroutine(optionsFadeRoutine);
                }
                optionsFadeRoutine = StartCoroutine(FadeOutPanel(optionsPanel, desiredAlpha, group));
            }
        }
    }

    IEnumerator WaitForFrame() {
        yield return new WaitForSeconds(0.1f);
        SetVolume(currentSettings.volume);
    }

    IEnumerator FadeOutPanel(Image panel, float desiredAlpha, CanvasGroup group) {

        isFading = true;

        for (float f = 1f; f >= desiredAlpha; f -= 0.1f) {
            float newAlpha = group.alpha;
            newAlpha = f;
            group.alpha = newAlpha;
            yield return new WaitForSeconds(0.05f);
        }

        isFading = false;

        optionsDisplayed = false;

        optionsPanel.gameObject.SetActive(false);

        optionsFadeRoutine = null;
    }

    IEnumerator FadeInPanel(Image panel, float desiredAlpha, CanvasGroup group) {

        isFading = true;

        for (float f = 0f; f <= desiredAlpha; f += 0.1f) {
            float newAlpha = group.alpha;
            newAlpha = f;
            group.alpha = newAlpha;
            yield return new WaitForSeconds(0.05f);
        }

        isFading = false;

        optionsDisplayed = true;

        optionsFadeRoutine = null;
    }

    IEnumerator Fade(bool fadingIn = false)
    {
        isFading = true;

        Color desiredColor = fadingIn ? loaded : loading;

        float target = desiredColor.a;

        if (!fadingIn)
        {
            for (float f = 0f; f <= target; f += 0.1f)
            {
                Color currentColor = fadeIn.color;
                currentColor.a = f;
                fadeIn.color = currentColor;
                yield return new WaitForSeconds(0.05f);
            }
        } else {

            for (float f = 1f; f >= target; f -= 0.1f)
            {
                Color currentColor = fadeIn.color;
                currentColor.a = f;
                fadeIn.color = currentColor;
                yield return new WaitForSeconds(0.05f);
            }
        }

        isFading = false;

        yield return null;
    }

    IEnumerator FadeInAndOut(Action endAction = null, Action middleAction = null, bool fadeInto = false)
    {
        Time.timeScale = 1f;
        yield return StartCoroutine(Fade());
        countDown.StartCounting();
        middleAction?.Invoke();
        if (fadeInto)
        {
            yield return StartCoroutine(Fade(true));
            endAction?.Invoke();
        }
    }

    public void ShowLoadingScreen()
    {
        GlobalMethods.Show(fadeIn.gameObject);
    }

    public void StartLoadingAsBlack()
    {
        fadeIn.color = loading;
    }

    public void ResetFadeIn()
    {
        if (!isFading)
        {
            if (fadeOnceRoutine != null)
            {
                StopCoroutine(fadeOnceRoutine);
                fadeOnceRoutine = StartCoroutine(Fade(true));
            }
            fadeOnceRoutine = StartCoroutine(Fade(true));
        }
    }
}
