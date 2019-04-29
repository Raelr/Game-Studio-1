﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMeter : InitialisedEntity
{
    [SerializeField]
    Transform meter;

    [SerializeField]
    Renderer meterRenderer;

    [SerializeField]
    Color startColor;

    [SerializeField]
    Color endColor;

    float maxAmount;

    const int minAmount = 0;

    public override void Initialise() {

        base.Initialise();

        maxAmount = meter.localScale.x;

        Vector3 newScale = new Vector3(0f, meter.localScale.y, meter.localScale.z);

        meter.localScale = newScale;

        meterRenderer.material.color = startColor;
    }

    public void IncrementMeter(float incrementSpeed, bool reverse = false) {

        if (meter.localScale.x < maxAmount) {

            Vector3 newAmount = reverse ? new Vector3(meter.localScale.x - incrementSpeed, meter.localScale.y, meter.localScale.z)
            : new Vector3(meter.localScale.x + incrementSpeed, meter.localScale.y, meter.localScale.z);

            meter.localScale = Vector3.Lerp(meter.localScale, newAmount, incrementSpeed * Time.deltaTime);

            Vector3 normalisedSanity = meter.localScale.normalized;

            meterRenderer.material.color = Color.Lerp(meterRenderer.material.color, endColor, 0.03f * Time.deltaTime);
            
        }
    }
}
