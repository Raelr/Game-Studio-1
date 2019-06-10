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
        private void SetColour() {
            switch (value) {
                case 500:
                    meshRenderer.material.color = Color.white;
                    break;
                case 1000:
                    meshRenderer.material.color = Color.blue;
                    break;
                case 3000:
                    meshRenderer.material.color = Color.green;
                    break;
                case 5000:
                    meshRenderer.material.color = Color.yellow;
                    break;
            }

           
        }

        private void OnCollisionEnter(Collision col) {
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