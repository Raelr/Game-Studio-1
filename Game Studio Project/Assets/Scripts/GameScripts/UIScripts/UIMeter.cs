using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMeter : InitialisedEntity
{
    [SerializeField]
    Transform meter = null;

    [SerializeField]
    Renderer meterRenderer = null;

    [SerializeField]
    Color startColor = Color.white;

    [SerializeField]
    Color endColor = Color.white;

    float maxAmount;

    const int minAmount = 0;

    public override void Initialise() {

        base.Initialise();

        maxAmount = meter.localScale.x;

        meterRenderer.material.color = startColor;

       // ChangeMeterStatus(false);
    }

    public void IncrementMeter(float incrementSpeed, bool reverse = false) {

        if (meter.localScale.x > 0) {

            Vector3 newAmount;

            if (reverse) {
                newAmount = new Vector3(maxAmount - incrementSpeed, meter.localScale.y, meter.localScale.z);
                ChangeMeterScale(newAmount);
            } else {
                newAmount = new Vector3(maxAmount - incrementSpeed, meter.localScale.y, meter.localScale.z);
                ChangeMeterScale(Vector3.Lerp(meter.localScale, newAmount, incrementSpeed * Time.deltaTime));
            }

          //  meterRenderer.material.color = Color.Lerp(meterRenderer.material.color, endColor, Time.deltaTime / 5);
        }
    }

    public void ChangeMeterStatus(bool status) {

        if (status) {
            GlobalMethods.Show(meter.gameObject);
        } else {
            GlobalMethods.Hide(meter.gameObject);
        }
    }

    public void ChangeMeterColor(Color color) {
        endColor = color;
    }

    public void ChangeMeterScale(Vector3 newScale) {

        meter.localScale = newScale;
    }
}
