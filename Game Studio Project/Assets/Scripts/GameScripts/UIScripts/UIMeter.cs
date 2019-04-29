using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMeter : InitialisedEntity
{
    [SerializeField]
    Transform meter;

    float maxAmount;

    const int minAmount = 0;

    public override void Initialise() {

        base.Initialise();
        maxAmount = meter.localScale.x;
    }

    public void IncrementMeter(float incrementSpeed, bool reverse = false) {

        if (meter.localScale.x > minAmount) {

            Vector3 newAmount = reverse ? new Vector3(meter.localScale.x - incrementSpeed, meter.localScale.y, meter.localScale.z)
            : new Vector3(meter.localScale.x + incrementSpeed, meter.localScale.y, meter.localScale.z);

            meter.localScale = Vector3.Lerp(meter.localScale, newAmount, incrementSpeed * Time.deltaTime);
        }
    }
}
