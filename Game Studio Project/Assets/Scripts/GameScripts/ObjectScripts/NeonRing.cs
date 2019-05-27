using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {
    public class NeonRing : MonoBehaviour {
        public List<Color> ringCols;
        public Renderer neonRingRenderer;

        public Transform neonRing;
        private Transform player;
        private bool canRotate;

        private void Start() {

            transform.localScale *= Random.Range(0.3f, 1);
            canRotate = true;
            player = GameMaster.instance.Spawner.Player.transform;
            SetRingSize(0);
            Color randomCol = ringCols[Random.Range(0, ringCols.Count)];
            neonRingRenderer.material.SetColor("_AtmoColor", randomCol);
            StartCoroutine(GrowRing(1));
        }

        public void Update() {
            if (canRotate) {
               //FacePlayer();
            }
        }

        //private void FacePlayer() {
        //    transform.LookAt(player);
        //    transform.rotation *= Quaternion.Euler(0, -180, 0);
        //    float distance = Vector3.Distance(transform.position, player.position);
       
        //    if (distance<100) {
        //        canRotate = false;
        //    }
        //}

        private IEnumerator GrowRing(float length) {
            float timeElapsed = 0;

            while (timeElapsed < length) {
                SetRingSize(timeElapsed / length);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            SetRingSize(1);
        }

        private void SetRingSize(float size) {
            neonRing.transform.localScale = new Vector3(size, size, size);
        }

    }
}