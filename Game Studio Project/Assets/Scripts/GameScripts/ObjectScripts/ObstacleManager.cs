using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {
    public class ObstacleManager : MonoBehaviour {

        private static ObstacleManager _instance;
        public static ObstacleManager Instance { get { return _instance; } }

        //Dictionary<int, Obstacle> allObstacles = new Dictionary<int, Obstacle>();
        List<GameObject> allObstacles = new List<GameObject>();
        void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
        }

        public void AddObstacleToList(GameObject obstcale) {
            allObstacles.Add(obstcale);
        }

        public void ChangeSpeed(float speed) {
            foreach (GameObject obstacle in allObstacles) {
               
            }
        }
    }
}
