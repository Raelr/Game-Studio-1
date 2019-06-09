using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float shakeDuration = 0;

    [SerializeField]
    float shakeMagnitue = 0;

    public static CameraShake instance;

    Coroutine shake;

    private void Awake()
    {
        instance = this;
    }

    public void StopCameraShake()
    {
        if (shake != null)
        StopCoroutine(shake);
    }

    public void ShakeOnce()
    {
        if (shake != null)
        {
            StopCoroutine(shake);
        }

        shake = StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitue));
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
