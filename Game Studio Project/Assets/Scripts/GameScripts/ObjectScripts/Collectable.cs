using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {
    public class Collectable : MonoBehaviour {

        [SerializeField] private List<PointData> pointData;
        private MeshRenderer meshRenderer;
        private Obstacle obstacle;
        private int value;
        private Color colour;

        private void Awake() {
            meshRenderer = GetComponent<MeshRenderer>();
            obstacle = GetComponent<Obstacle>();
            SetValue();
            SetColour();
        }

        private void SetValue() {
            int roll = Random.Range(0, 100);
            float weightSum = 0;

            foreach (PointData data in pointData) {
                weightSum += data.chance * 100;
                if (roll <= weightSum) {
                    value = data.value;
                    colour = data.colour;
                    break;
                }
            }
        }
        private void SetColour()
        {
            meshRenderer.material.color = colour;           
        }

        private bool hit = false;

        private void OnCollisionEnter(Collision col)
        {
            Debug.Log(col.transform.name + " hit");
            if (col.transform.name != "Visuals") return;
            if (hit) return;
            hit = true;
            PlayerMaster.instance.HitRelic();
            StartCoroutine(Hit());
        }

        private IEnumerator Hit ()
        {
            yield return new WaitForSeconds(0.1f);
            PlayerMaster.instance.UpdateScore(value);
            obstacle.BackToPool();
        }

        [System.Serializable]
        public struct PointData {
            public int value;
            public float chance;
            public Color colour;
        }
    }
}