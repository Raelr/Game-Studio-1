using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPoints : MonoBehaviour
{
    private TextMeshPro pointsText;
    private Transform source;
    private Vector3 offsetPos;
    private float time = 0;

    private void Awake() {
        pointsText = GetComponent<TextMeshPro>();
    }

    private void Update() {
        if (source != null) {
            transform.position = source.position;
        }
    }

    public void Initialise(string text, Transform source) {
        this.source = source;
        offsetPos = new Vector3(0,10,0);
        pointsText.text = text;

        StartCoroutine(TransformText());
    }

    private IEnumerator TransformText() {
        float elapsedTime = 0;

        while (elapsedTime < time) {
            offsetPos += 5 * Time.deltaTime * Vector3.up;
            transform.position = offsetPos + source.position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Die();
    }

    private void Die() {
        Destroy(this.gameObject);
    }
}
