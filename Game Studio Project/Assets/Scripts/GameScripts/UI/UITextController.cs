using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextController : InitialisedEntity
{
    [SerializeField]
    TextMeshPro livesText;

    public override void Initialise() {

        base.Initialise();

    }

    public void UpdateText(string description, int value) {

        livesText.text = description + value;
    }
}
