﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITime : InitialisedEntity
{
    [SerializeField]
    private TextMeshPro timeText = null;
    
    private float time = 0;

    public override void Initialise() {
        
        time = 0;

        ChangeTextStatus(false);
    }

    public void IncrementTime() {

        time += Time.deltaTime;
        timeText.text = "Time: " + time.ToString("F2");
    }

    public void ChangeTextStatus(bool value) {

        timeText.enabled = value;
    }

    public void ChangeTextColor(Color color) {

        timeText.color = color;
    }
}
