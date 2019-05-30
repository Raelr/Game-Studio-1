using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AlternativeArchitecture {
    // Handles all actions which the entity can take. This is an example of a player's specific controller.
    public class MovementController : InitialisedEntity {

        public delegate void OnCollisionhandler();

        public delegate void OnNearMissHandler();
		
        public delegate void OnRingHitHandler();

        public delegate void OnTimeChangeHandler(float speed);

        public OnCollisionhandler onCollision;

        public OnNearMissHandler onNearMiss;
		
        public OnRingHitHandler onRingHit;

        public OnTimeChangeHandler onTimeChange;

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

        [Header("Dash Properties")]
        [SerializeField] AudioClip[] dashClips;
        [SerializeField] private float forceStep = 0.5f;
        private float acceleration = 1;
        private float currentSpeed = 1;
        AudioSource dashAudio;

        [Header("Animation properties")]
        [SerializeField] AnimationCurve rotationAnim;
        [SerializeField] AnimationCurve dashAnim;

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
            dashAudio = GetComponent<AudioSource>();
            player = transform.Find("Visuals");
            physics.Initialise();

            physics.onCollision += onPlayerCollision;
            physics.onNearMiss += OnPlayerNearMiss;
            physics.onRingHit += OnPlayerRingHit;
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

        public void RotateEntity3(Vector2 input) {



            rotationX += input.x * Time.deltaTime + (accelerationX * input.x * Time.deltaTime);
            rotationY += input.y * Time.deltaTime + (accelerationY * input.y * Time.deltaTime);


            //Debug.Log(input.y* Time.deltaTime + (accelerationY * lastDir.y));
            //rotationX = rotationX >= 360 ?
            rotationY = Mathf.Clamp(rotationY, -70, 70);
            rotationX = Mathf.Clamp(rotationX, -180, 180);

            //Debug.Log(-stepRotation * input.y);
            transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
            //player.transform.localRotation = Quaternion.Euler(new Vector3(-stepRotation * input.y * 2, stepRotation * input.x * 2, -rotationX));



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

        public void RotateEntity(Vector2 input) {
            rotationX += input.x * stepRotation * Time.deltaTime * force * acceleration;
            rotationY += input.y * stepRotation * Time.deltaTime * force * acceleration;

            rotationY = Mathf.Clamp(rotationY, -70, 70);

            //Debug.Log(-stepRotation * input.y);
            transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
            player.transform.localRotation = Quaternion.Euler(new Vector3(-stepRotation * input.y * 2, stepRotation * input.x * 2, -rotationX));

            if (input.x != 0 || input.y != 0) {
                acceleration += accelerationStepping;
                if (acceleration >= maxAcceleration)
                    acceleration = maxAcceleration;
            }
            else {
                acceleration = accelerationBase;
            }

            if (rotationX < -60) {
                stepRotation += input.x * 10f * Time.deltaTime;
            }
            if (rotationX > 60) {
                stepRotation += -input.x * 10f * Time.deltaTime;
            }

            stepRotation = Mathf.Clamp(stepRotation, 0, 10);
        }

        public void onPlayerCollision() {
            onCollision?.Invoke();
        }

        public void OnPlayerNearMiss() {
            if (!isDashing) {
                StartCoroutine(InputPrompt("ask"));
            }
        }

        public void OnPlayerRingHit() {
            Debug.Log("ring hit");
           // if (!isDashing) {
                StartCoroutine(InputPrompt("auto"));
            //}
        }


        private void StartRetreat() {
            StartCoroutine(Retreat());
        }

        private IEnumerator InputPrompt(string prompt) {
            Debug.Log("ring: " + prompt);
            stepRotation = 1;
            dashAudio.clip = dashClips[0];
            dashAudio.Play();
            GamePooler.instance.SetObstacleSpeed(0.1f);
            onTimeChange(0.1f);
            //yield return new WaitForSeconds(0.5f);
            float elapsedTime = 0;
            float time = 2f;
            float speedBoost = 0;
            bool successfulDash = false;
            bool hasPromptPlayed = false;

            while (elapsedTime < time) {

                elapsedTime += Time.deltaTime;

                speedBoost += 1 * Time.deltaTime;
                Debug.Log(elapsedTime + " " + prompt);
                if (Input.GetMouseButtonDown(1) && elapsedTime > 0.5f || prompt == "auto") {
                    
                    dashAudio.clip = dashClips[1];
                    dashAudio.Play();
                    successfulDash = true;
                    StopCoroutine(Retreat());
                    StopCoroutine(Dash(0));
                    StartCoroutine(Dash(speedBoost));
                    StartCoroutine(TestRotation());
                    onNearMiss?.Invoke();
                    break;
                }
                else if (Input.GetMouseButtonDown(1)) {
                    successfulDash = false;
                    break;
                }
                yield return null;
            }

            if (!successfulDash) {
                dashAudio.Stop();
                GamePooler.instance.SetObstacleSpeed(currentSpeed);
                onTimeChange(1f);
                stepRotation = 10;
            }
        }

        private IEnumerator TestRotation() {

            float elapsedTime = 0;
            float time = 1.1f;

            Quaternion sourceOrientation = player.transform.localRotation;
            float sourceAngle = 0;
            float targetAngle = 1080f + sourceAngle; // Source +/- 1800
            Vector3 targetAxis = new Vector3(1, 0, 0);

            while (elapsedTime < time) {
                yield return null;

                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / time;

                //hello
                float currentAngle = Mathf.Lerp(sourceAngle, targetAngle, rotationAnim.Evaluate(progress));
                player.transform.localRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            }

        }

        private IEnumerator Dash(float speedBoost) {
            Vector3 startPos = player.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x, startPos.y, 15);

            currentSpeed += forceStep * speedBoost;
            float elapsedTime = 0;
            float time = 0.1f;

            isDashing = true;
            CameraEffects.instance.DashOn();
            GamePooler.instance.SetObstacleSpeed(currentSpeed + 5);

            while (elapsedTime < time) {
                float progress = elapsedTime / time;
                player.transform.localPosition = Vector3.Lerp(startPos, endPos, dashAnim.Evaluate(progress));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            player.transform.localPosition = endPos;
            StartCoroutine(Cooldown(0.3f, StartRetreat));
        }

        private IEnumerator Retreat() {
            Vector3 startPos = player.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x, startPos.y, 10);

            isDashing = false;
            float elapsedTime = 0;
            float time = 0.5f;
            float speedScale = currentSpeed + 5;

            while (elapsedTime < time) {
                player.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / time);
                speedScale = Mathf.Lerp(currentSpeed +5, currentSpeed, elapsedTime/time);
                stepRotation = Mathf.Lerp(1, 10, elapsedTime/time);
                GamePooler.instance.SetObstacleSpeed(speedScale);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            CameraEffects.instance.DashOff();
            GamePooler.instance.SetObstacleSpeed(currentSpeed);
            onTimeChange(1f);
            Debug.Log(currentSpeed);
            player.transform.localPosition = endPos;

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
