using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float shakeDuration;

    [SerializeField]
    float shakeMagnitue;

    public static CameraShake instance;

    private void Awake()
    {
        instance = this;
    }

    public void StopCameraShake()
    {
        StopCoroutine(ShakeCamera(shakeDuration, shakeMagnitue));
    }

    public void ShakeOnce()
    {
        StopCoroutine(ShakeCamera(shakeDuration, shakeMagnitue));
        StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitue));
    }

    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < duration) {

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
