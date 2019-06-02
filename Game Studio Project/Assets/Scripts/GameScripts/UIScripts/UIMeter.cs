using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMeter : InitialisedEntity
{
    [SerializeField]
    Transform meter;

    [SerializeField]
    Renderer meterRenderer;

    [SerializeField]
    Color startColor = Color.white;

    [SerializeField]
    Color endColor = Color.white;

    float maxAmount;

    const int minAmount = 0;

    public override void Initialise() {

        base.Initialise();

        maxAmount = meter.localScale.x;

        Vector3 newScale = new Vector3(0f, meter.localScale.y, meter.localScale.z);

        meter.localScale = newScale;

        meterRenderer.material.color = startColor;

        ChangeMeterStatus(false);
    }

    public void IncrementMeter(float incrementSpeed, bool reverse = false) {

        if (meter.localScale.x <= maxAmount) {

            Vector3 newAmount = reverse ? new Vector3(meter.localScale.x - incrementSpeed, meter.localScale.y, meter.localScale.z)
            : new Vector3(meter.localScale.x + incrementSpeed, meter.localScale.y, meter.localScale.z);

            meter.localScale = Vector3.Lerp(meter.localScale, newAmount, Time.deltaTime / incrementSpeed);

            meterRenderer.material.color = Color.Lerp(meterRenderer.material.color, endColor, Time.deltaTime / 10);
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
}
