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
    

    public override void Initialise() {

        base.Initialise();

        ChangeTextStatus(false);

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

    public void ShowPoints(float value, Transform source) {
        UIPoints pointsObj = Instantiate(points, source);
        pointsObj.Initialise(value.ToString(), source);
        Debug.Log("I should be alive bitches");
    }

    public void ChangeTextColor(Color color) {

        pointSystem.color = color;
    }
}
