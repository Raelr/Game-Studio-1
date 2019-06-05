﻿using System.Collections;
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

    public void ChangeTextStatus(bool value) {

        pointSystem.enabled = value;
    }

    public void GainPoints(float value) {
        int rounded = (int)value;
        pointSystem.text = "Score: " + rounded.ToString();
    }

    public void ChangeTextColor(Color color) {

        pointSystem.color = color;
    }
}
