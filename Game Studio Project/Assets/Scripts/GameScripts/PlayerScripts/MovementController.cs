using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AlternativeArchitecture {
    // Handles all actions which the entity can take. This is an example of a player's specific controller.
    public class MovementController : InitialisedEntity {

        public delegate void OnCollisionhandler();

        public delegate void OnNearMissHandler();

        public OnCollisionhandler onCollision;

        public OnNearMissHandler onNearMiss;

        // The controller should keep track of all physics components (since it is the only compone≤nt which needs to interface with physics)
        [Header("Physics")]
        [SerializeField]
        PhysicsController physics;

        [Header("Physics Properties")]
        [SerializeField] private float force = 25f;
        [SerializeField] private float maxDistance = 8;
        [SerializeField] private bool invertMovement = false;
        [SerializeField] private float minRotation = -30;
        [SerializeField] private float maxRotation = 30;
        [SerializeField] private float stepRotation = 0.1f;
        [SerializeField] private float maxAcceleration = 5;
        [SerializeField] private float accelerationStepping = 1;
        [SerializeField] private float accelerationBase = 1;
        private float acceleration = 1;

        private float rotationX;
        private float rotationY;
        private Vector3 lastPosition;
        private bool isDashing;

        [Header("Player Bounds")]
        private float xBounds = 100f;
        private float yBounds = 12f;

        private Transform player;

        // Initialises all variables and gets the physics component. 
        public override void Initialise() {

            base.Initialise();

            physics = GetComponent<PhysicsController>();
            player = transform.Find("Visuals");
            physics.Initialise();

            physics.onCollision += onPlayerCollision;
            physics.onNearMiss += OnPlayerNearMiss;
        }

        // DEPRECIATED -- 

        // Keeping old movement componenet as a reference

        //  public void MoveEntity(Vector2 targetPos) {
        //      if (!invertMovement)
        //      {
        //          targetPos *= -1;
        //          targetPos.y += 2;
        //      }
        //      //targetPos *= -1;
        //      //targetPos.y += 2;

        //float dist = Vector3.Distance(targetPos, player.position);
        //Vector2 dir = GlobalMethods.GetDirection(player.position, targetPos);
        //Vector2 velocity = dir * (force * (dist / maxDistance));

        //Vector3 nextPosition = (Vector2)player.position + GlobalMethods.Normalise(dir);

        //Clamps velocity to make sure player stays within the set bounds
        //velocity.x = GlobalMethods.WithinBounds(nextPosition.x, -xBounds, xBounds) ? velocity.x : 0;
        //velocity.y = GlobalMethods.WithinBounds(nextPosition.y, -yBounds, yBounds) ? velocity.y : 0;

        //physics.AddForce(velocity);
        //}

        // -- DEPRECIATED

        private float accelerationX = 0.01f;
        private float accelerationY = 0.01f;
        private Vector2 lastDir = new Vector2();

        public void RotateEntity(Vector2 input) {



            rotationX += input.x * Time.deltaTime + (accelerationX * input.x * Time.deltaTime);
            rotationY += input.y * Time.deltaTime + (accelerationY * input.y * Time.deltaTime);

            Debug.Log(lastDir.y);
            //Debug.Log(input.y* Time.deltaTime + (accelerationY * lastDir.y));
            //rotationX = rotationX >= 360 ? 
            rotationY = Mathf.Clamp(rotationY, -70, 70);

            //Debug.Log(-stepRotation * input.y);
            transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
            player.transform.localRotation = Quaternion.Euler(new Vector3(-stepRotation * input.y * 2, stepRotation * input.x * 2, -rotationX));



            accelerationX *= input.x != 0 ? 2 : 0.9f;
            accelerationX = Mathf.Clamp(accelerationX, 0.01f, maxAcceleration);

            accelerationY *= input.y != 0 ? 2 : 0.9f;
            accelerationY = Mathf.Clamp(accelerationY, 0.01f, maxAcceleration);

            //if (input.x != 0 || input.y != 0) {
            //    acceleration += accelerationStepping;
            //    if (acceleration >= maxAcceleration)
            //        acceleration = maxAcceleration;
            //}
            //else if (acceleration > 0) {
            //    acceleration -= accelerationStepping;
            //} 
            //else  {
            //    acceleration = accelerationBase;
            //}
        }

        public void onPlayerCollision() {
            onCollision?.Invoke();
        }

        public void OnPlayerNearMiss() {
            if (!isDashing) {
                StartCoroutine(Dash());
            }
        }

        private void StartRetreat() {
            StartCoroutine(Retreat());
        }

        private IEnumerator Dash() {
            Vector3 startPos = player.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x, startPos.y, 20);
            float elapsedTime = 0;
            float time = 0.2f;
            isDashing = true;
            CameraEffects.instance.DashOn();
            GamePooler.instance.SetObstacleSpeed(10f);

            while (elapsedTime < time) {
                player.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            player.transform.localPosition = endPos;
            StartCoroutine(Cooldown(1, StartRetreat));
        }

        private IEnumerator Retreat() {
            Vector3 startPos = player.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x, startPos.y, 10);
            float elapsedTime = 0;
            float time = 1f;
            float speedScale = 10f;

            while (elapsedTime < time) {
                player.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / time);
                speedScale = Mathf.Lerp(10, 1, elapsedTime/time);
                GamePooler.instance.SetObstacleSpeed(speedScale);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            CameraEffects.instance.DashOff();
            GamePooler.instance.SetObstacleSpeed(1);
            player.transform.localPosition = endPos;
            isDashing = false;
        }

        private IEnumerator Cooldown(float time, Action action) {
            float elapsedTime = 0;
            isDashing = true;

            while (elapsedTime < time) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            action.Invoke();
        }

    }
}
