using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextController : InitialisedEntity
{
    [SerializeField]
    TextMeshPro livesText;
    [SerializeField]
    TextMeshPro pointSystem;

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
        pointSystem.text = "Score: " + value.ToString();
    }
}
