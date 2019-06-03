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

    public override void Initialise() {

        base.Initialise();

        ChangeTextStatus(false);

    }

    public void UpdateText(string description, int value) {

        livesText.text = description + value;
    }

    public void ChangeTextStatus(bool value) {

        livesText.enabled = value;
    }

    public void GainPoints(float value) {
        int rounded = (int)value;
        pointSystem.text = "Score: " + rounded.ToString();
    }

    public void ChangeTextColor(Color color) {

        pointSystem.color = color;
    }
}
