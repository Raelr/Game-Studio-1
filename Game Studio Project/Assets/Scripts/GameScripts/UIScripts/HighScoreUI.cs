using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{

    public static HighScoreUI instance;


    int highScore = 0, score = 0;
    bool highScoreSet = false;

    bool beatenHighScore = false;


    public Transform barScalar;
    public TextMesh percentNum, percentSymbol, highScoreText;
    public Renderer barRenderer, baseRenderer, arrowRenderer;

    public TextMesh highScoreNum;

    public Color winCol;

    public Animator highScoreAnim;

    public AudioSource newHighScoreSound;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        RefreshDisplay();
    }

    /*int temp_score = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetHighScore(1500);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            temp_score += 123;
            SetScore(temp_score);
        }
    }*/



    public void SetHighScore (int hs)
    {
        highScore = hs;
        highScoreSet = true;
        UpdateHighScoreNum(highScore);
        ShowProgress();
        RefreshDisplay();
    }

    public void SetScore (int s)
    {
        if (!highScoreSet)
        {
            Debug.LogError("High score needs to be set first. I'm not going to do anything now.");
            return;
        }

        score = s;
        RefreshDisplay();
    }


    void RefreshDisplay()
    {
        if (!highScoreSet) return;
        float percent = ((float)score / (float)highScore) * 100f;
        float roundedPercent = Mathf.Round(percent);

        if (roundedPercent >= 100)
        {
            if (!beatenHighScore)
            {
                beatenHighScore = true;
                HighScoreBeaten();
            }
            else {
                UpdateHighScoreNum(score);
            }
            roundedPercent = 100;
        }

        percentNum.text = ""+roundedPercent;
        barScalar.transform.localScale = new Vector3(roundedPercent / 100, 1, 1);
    }

    void UpdateHighScoreNum(int value) {
        highScoreNum.text = "" + value;
    }

    void HighScoreBeaten ()
    {
        //set text to yellow

        // do some confetti suprise
        percentNum.color = winCol;
        percentSymbol.color = winCol;
        highScoreText.color = winCol;
        barRenderer.material.color = winCol;
        highScoreText.text = "New High\nScore!";

        newHighScoreSound.Play();

        highScoreAnim.SetBool("PlayAnim", true);
        StartCoroutine(TurnOffAnim());
        StartCoroutine(HideProgress());
    }

    IEnumerator TurnOffAnim ()
    {
        yield return new WaitForSeconds(0.5f);
        highScoreAnim.SetBool("PlayAnim", false);
    }

    IEnumerator HideProgress() {
        yield return new WaitForSeconds(3f);
        percentNum.gameObject.SetActive(false);
        percentSymbol.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        barRenderer.enabled = false;
        baseRenderer.enabled = false;
        arrowRenderer.enabled = false;
    }

    void ShowProgress() {
        percentNum.gameObject.SetActive(true);
        percentSymbol.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        barRenderer.enabled = true;
        baseRenderer.enabled = true;
    }

}
