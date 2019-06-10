using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextController : InitialisedEntity
{
    [SerializeField]
    TextMeshPro livesText = null;
    [SerializeField]
    TextMeshPro pointSystem = null;
    [SerializeField]
    TextMeshProUGUI score = null;

    [Header("Points Components")]
    [SerializeField]
    UIPoints points = null;
    [SerializeField]
    UIPoints combo = null;


    public override void Initialise() {

        base.Initialise();

        ChangeTextStatus(false);
        points.gameObject.SetActive(false);
    }

    public void UpdateText(string description, int value = -1) {

        if (value != -1 && livesText != null)
        {
            livesText.text = description + value;
        } else
        {
            score.text = description;
        }
    }

    public string GetTextValue()
    {
        string[] score = pointSystem?.text.Split(' ');
        return score[1];
    }

    public string GetScoreValue()
    {
        return score.text;
    }

    public void ChangeTextStatus(bool value) {

        pointSystem.enabled = value;
    }

    public void GainPoints(float value) {
        int rounded = (int)value;
        pointSystem.text = "Score: " + rounded.ToString();
        HighScoreUI.instance.SetScore((int)value);
    }

    public void ShowPoints(float pointValue, Transform source) {
        points.gameObject.SetActive(true);
        points.Initialise(pointValue, source, 2);
    }

    public void ShowCombo(int comboValue, Transform source) {
        Debug.Log("Combo Showing");
        combo.gameObject.SetActive(true);
        combo.Initialise(comboValue, source, -2);
    }

    public void ChangeTextColor(Color color) {

        pointSystem.color = color;
    }
}
