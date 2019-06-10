using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPoints : MonoBehaviour {

    private ParticleSystem particleSystem;
    private TextMeshPro pointsText;
    private Transform source;
    private Vector3 offsetPos;
    private float time = 1;

    [Header("Animation Curves")]
    [SerializeField] AnimationCurve scaleAnim;
    [SerializeField] AnimationCurve vectorAnim;

    [Header("Text properties")]
    [SerializeField] pointColours[] pointColours;
    private pointColours currentColours;
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    private void Awake() {
        pointsText = GetComponent<TextMeshPro>();
        particleSystem = GetComponent<ParticleSystem>();
        gradient = new Gradient();
    }

    public void Initialise(float value, Transform source) {
        SetColours(value);
        SetParticleSystem();
        SetTextColour();
        this.source = source;
        offsetPos = new Vector3(0, 0, -3);
        pointsText.text = value.ToString();

        StopCoroutine(ScaleDown());
        StopCoroutine(TransformText());
        StartCoroutine(TransformText());
    }

    private void SetColours(float value) {
        if (value < 500) {
            Debug.Log("Should be right");
            currentColours = pointColours[0];
        }
        else if (value >= 500 && value < 1000) {
            currentColours = pointColours[1];
        }
        else if (value >= 1000 && value < 5000) {
            currentColours = pointColours[2];
        }
        else if (value >= 5000) {
            currentColours = pointColours[3];
        }
    }

    private void SetParticleSystem() {
        var main = particleSystem.main;

        colorKey = new GradientColorKey[2];
        colorKey[0].color = currentColours.startColour;
        colorKey[0].time = 0.0f;
        colorKey[1].color = currentColours.endColour;
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.5f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
        main.startColor = gradient;

        particleSystem.Emit(50);
    }

    private void SetTextColour() {
        pointsText.color = currentColours.startColour;
    }

    private IEnumerator TransformText() {
        float elapsedTime = 0;

        while (elapsedTime < time) {
            offsetPos = Vector3.Lerp(new Vector3(0,0,-3), new Vector3(0,1,-3), vectorAnim.Evaluate(elapsedTime/time));
            transform.position = offsetPos + source.position;
            transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.4f,0.4f,0.4f), scaleAnim.Evaluate(elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ScaleDown());
    }

    private IEnumerator ScaleDown() {
        float elapsedTime = 0;

        while (elapsedTime < 0.5) {
            transform.localScale = Vector3.Lerp(new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.2f, 0.2f, 0.2f), scaleAnim.Evaluate(elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Die();
    }

    private void Die() {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct pointColours {
    public Color startColour;
    public Color endColour;
}
