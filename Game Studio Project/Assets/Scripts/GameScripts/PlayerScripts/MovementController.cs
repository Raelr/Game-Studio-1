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
        [SerializeField] private float stepRotation = 0.1f;
        [SerializeField] private float maxAcceleration = 5;
        [SerializeField] private float accelerationStepping = 1;
        [SerializeField] private float accelerationBase = 1;
        

        [Header("Dash Properties")]
        [SerializeField] AudioClip[] dashClips = null;
        [SerializeField] Renderer dashIcon;
        public float dashPitchMin, dashPitchMax;
        public float dashBuildUpVolume, dashBurstVolume;

        [SerializeField] private float forceStep = 0.5f;
        private float acceleration = 1;
        private float currentSpeed = 1;
        private float pointMultiplier = 1;
        private float score = 0;
        private int currentLevel = 1;
        private int comboMultiplier;
        private IEnumerator comboCoroutine;

        AudioSource dashAudio;

        [Header("Animation properties")]
        [SerializeField] AnimationCurve rotationAnim = null;
        [SerializeField] AnimationCurve dashAnim = null;

        private float rotationX;
        private float rotationY;
        private Vector3 lastPosition;
        private bool isDashing;
        private bool isRetreating;
        private bool comboing;
        private bool comboTriggered;

        private Transform player;

        public float PointMultiplier { get { return pointMultiplier + ((float)currentLevel*0.5f); } }
        
        private bool invertY = false;

        // Initialises all variables and gets the physics component.
        public override void Initialise() {

            base.Initialise();

            physics = GetComponent<PhysicsController>();
            dashAudio = GetComponent<AudioSource>();
            player = transform.Find("Visuals");
            dashIcon = player.Find("DashIcon").GetComponent<Renderer>();
            dashIcon.enabled = false;
            physics.Initialise();

            physics.onCollision += onPlayerCollision;
            physics.onNearMiss += OnPlayerNearMiss;
            physics.onRingHit += OnPlayerRingHit;

            comboCoroutine = ComboTimer();


            if (PlayerPrefs.HasKey("INVERT_Y")) {
                invertY = true;
            }
            else {
                invertY = false;
            }
        }

        private float accelerationX = 0.01f;
        private float accelerationY = 0.01f;

        public void RotateEntity3(Vector2 input) {
            if (invertY)
                input.y *= -1;


            rotationX += input.x * Time.deltaTime + (accelerationX * input.x * Time.deltaTime);
            rotationY += input.y * Time.deltaTime + (accelerationY * input.y * Time.deltaTime);

            rotationY = Mathf.Clamp(rotationY, -70, 70);
            rotationX = Mathf.Clamp(rotationX, -180, 180);

            transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));

            accelerationX *= input.x != 0 ? 2 : 0.9f;
            accelerationX = Mathf.Clamp(accelerationX, 0.01f, maxAcceleration);

            accelerationY *= input.y != 0 ? 2 : 0.9f;
            accelerationY = Mathf.Clamp(accelerationY, 0.01f, maxAcceleration);

        }

        public void MultiplyPoints(Vector2 input) {
            pointMultiplier += 0.1f * Time.deltaTime;

            score += 5 * Time.deltaTime * PointMultiplier;
            UIMaster.instance.UpdatePoints(score);
        }

        public void RotateEntity(Vector2 input) {

            if (invertY)
                input.y *= -1;

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

        void FixedUpdate () {
            
            if (Input.GetKeyDown(KeyCode.Y) && allowToggle) {
                allowToggle = false;
                ToggleInvertY();

                StartCoroutine(ResetToggle());
                Debug.Log("!!!");
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                Debug.Log("Multiplier: " + PointMultiplier);
                Debug.Log("Level: " + currentLevel);
            }
        }

        IEnumerator ResetToggle () {
            yield return new WaitForSeconds(0.5f);
            allowToggle = true;
        }

        private bool allowToggle = true;


        private void ToggleInvertY () {
            Debug.Log("toggled " + gameObject.name);

            invertY = !invertY;
        
            if (invertY) {
                PlayerPrefs.SetInt("INVERT_Y", 1);
            }
            else {
                PlayerPrefs.DeleteKey("INVERT_Y");
            }
        }

        public void onPlayerCollision() {
            if (!isDashing) {
                onCollision?.Invoke();
                pointMultiplier = 1;
            }
        }

        public void OnPlayerNearMiss() {
            Combo();
            if (!isDashing) {
                StartCoroutine(InputPrompt("ask"));
                GainPoints(250);
            }
            if (isRetreating) {
                pointMultiplier += 2;
                GainPoints(500);
            }
        }

        public void OnPlayerRingHit() {
            Combo();
            StartCoroutine(InputPrompt("auto"));
            GainPoints(50);
        }

        public void GainPoints(float value) {
            int points = (int)(value * PointMultiplier * comboMultiplier);
            score += points;
            UIMaster.instance.UpdatePoints(score);
            UIMaster.instance.ShowPoints(points, player);
        }

        public void Combo() {
            if (comboing) {
                comboMultiplier++;
                if (comboMultiplier > 1) {
                    UIMaster.instance.ShowCombo(comboMultiplier, player);
                }
            }
            comboing = true;
            StopCoroutine(comboCoroutine);
            comboCoroutine = ComboTimer();
            StartCoroutine(comboCoroutine);
        }

        private IEnumerator ComboTimer() {
            float elpasedTime = 0;

            while (elpasedTime<1.5f) {
                elpasedTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("Reseting combo");
            comboMultiplier = 1;
            comboing = false;
        }


        private void StartRetreat() {
            StartCoroutine(Retreat());
        }

        private IEnumerator InputPrompt(string prompt) {


            //Debug.Log("ring: " + prompt);
            stepRotation = 1;
            dashAudio.clip = dashClips[0];
            dashAudio.pitch = UnityEngine.Random.Range(dashPitchMin, dashPitchMax);
            dashAudio.volume = dashBuildUpVolume;
            dashAudio.Play();
            GamePooler.instance.SetObstacleSpeed(0.1f);
            onTimeChange(0.1f);
            //yield return new WaitForSeconds(0.5f);
            float elapsedTime = 0;
            float time = 2f;
            float speedBoost = 0;
            bool successfulDash = false;

            while (elapsedTime < time) {

                elapsedTime += Time.deltaTime;

                if (elapsedTime > 0.5f && !dashIcon.enabled) {
                    dashIcon.enabled = true;
                }

                speedBoost += 1 * Time.deltaTime;
                if (Input.GetButtonDown("Fire1") && elapsedTime > 0.5f || prompt == "auto") {

                    if (prompt == "ask")
                    {
                        HapticEngine.instance.Vibrate(HapticEffect.DASH_FIRE);
                    }


                    dashAudio.clip = dashClips[1];
                    dashAudio.pitch = UnityEngine.Random.Range(dashPitchMin, dashPitchMax);
                    dashAudio.volume = dashBurstVolume;
                    dashAudio.Play();
                    successfulDash = true;
                    dashIcon.enabled = false;
                    StopCoroutine(Retreat());
                    StopCoroutine(Dash(0));
                    StartCoroutine(Dash(speedBoost));
                    StartCoroutine(TestRotation());
                    onNearMiss?.Invoke();
                    break;
                }
                else if (Input.GetButtonDown("Fire1")) {
                    successfulDash = false;
                    break;
                }
                yield return null;
            }
            dashIcon.enabled = false;
            if (!successfulDash) {
                dashAudio.Stop();
                dashIcon.enabled = false;
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
            StartCoroutine(Cooldown(0.1f, StartRetreat));
        }

        private IEnumerator Retreat() {
            Vector3 startPos = player.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x, startPos.y, 10);

            isRetreating = true;
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
            player.transform.localPosition = endPos;
            isRetreating = false;

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

        public void UpdateLevelData(int level) {
            currentLevel = level;
        }

    }
}
