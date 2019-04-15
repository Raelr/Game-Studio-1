using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISpeed : InitialisedEntity
{
    [SerializeField]
    private TextMeshPro speedText;

    private float speed;

    public override void Initialise()
    {
        speed = 100000;
    }

    public void IncrementSpeed(float amount) {
        speed += amount;
        speedText.text = "Speed Km/h: " + speed.ToString("F0");
    }
}
