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
    [SerializeField] Color[] 

    private void Awake() {
        pointsText = GetComponent<TextMeshPro>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Initialise(string text, Transform source) {
        particleSystem.Emit(50);
        this.source = source;
        offsetPos = new Vector3(0, 0, -3);
        pointsText.text = text;

        StopCoroutine(ScaleDown());
        StopCoroutine(TransformText());
        StartCoroutine(TransformText());
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

public struct pointColours {
    public Color startColour;
    public Color endColour;
}
