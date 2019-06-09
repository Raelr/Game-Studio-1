using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPoints : MonoBehaviour {
    private TextMeshPro pointsText;
    private Vector3 source;
    private Vector3 offsetPos;
    private float time = 3;

    private void Awake() {
        pointsText = GetComponent<TextMeshPro>();
    }

    public void Initialise(string text, Transform source) {
        this.source = source.position;
        offsetPos = new Vector3(0, 0, 0);
        pointsText.text = text;

        StopCoroutine(TransformText());
        StartCoroutine(TransformText());
    }

    private IEnumerator TransformText() {
        float elapsedTime = 0;

        while (elapsedTime < time) {
            offsetPos += 5 * Time.deltaTime * new Vector3(0,0,-1);
            transform.position = offsetPos + source;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Die();
    }

    private void Die() {
        gameObject.SetActive(false);
    }
}
