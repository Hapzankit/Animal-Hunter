using Cinemachine;
using HapzsoftGames;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace TheBox
{
    public class BulletTimeManager : MonoBehaviour
    {
        public static BulletTimeManager instance;

        [Header("Bullet time Scriptable Objects")]
        public BulletTimeScriptableObject bulletTimeValues = default;

        [Header("Prefabs")]
        [SerializeField] GameObject BulletTimePrefab = default;
        [SerializeField] GameObject BulletTimeCameraPrefab = default;
        //[SerializeField] GameObject TargetCameraPrefab = default;

        [Header("Camera LookAt")]
        [SerializeField] Transform LookAtTarget = default;
                
        /// <summary>
        /// We keep the instance of the prefabs during all the process and destroy it at the end.
        /// </summary>
        public GameObject bulletTimeInstance = default;
        public GameObject bulletTimeCameraInstance = default;
        GameObject targetCameraInstance = default;

        /// <summary>
        /// DollyCart scripts from the cameras prefabs instance
        /// </summary>
        CinemachineDollyCart bulletDollyCartInstance = default;
        CinemachineDollyCart targetDollyCartInstance = default;

        /// <summary>
        /// Virtual camera scripts from the camera prefabs instance
        /// </summary>
        [SerializeField] CinemachineVirtualCamera bulletVirtualCameraInstance = default;
        //CinemachineVirtualCamera targetVirtualCameraInstance = default;

        // Starting point of the bullet
        Vector3 startPoint = default;
        public GameObject bulletRotation;

        // Target Raycast
        RaycastHit ray = default; 
        
        // Distance between the starting point of the bullet and the target.
        float distanceFromTarget = default;

        // Current distance between the bullet and the target.
        float bulletTargetDistance = default;

        // Update each frame it's the position on the curve related to the distance between the starting
        // point and the bullet current position.
        float key = default;

        // Prevent from calling multiple time the coroutin use to wait before destroying camera and bullet.
        public bool targetHited = default;

        public SimpleRifleController simpleRifleController;

        private Camera BulletEffectCamera;




        private void Awake()
        {
            // Singleton pattern
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            // Enable the script only when need it to launch a bullet time.
            //enabled = false;

            simpleRifleController = FindAnyObjectByType<SimpleRifleController>(); 
        }

        /// <summary>
        /// Launch a bullet in slow motion.
        /// </summary>
        /// <param name="startPoint">Bullet starting position</param>
        /// <param name="ray">Raycast on the target</param>
        public void LaunchBulletTime(Vector3 startPoint, RaycastHit ray)
        {
            if (enabled)
            {
                Debug.Log("Already in bullet time!");
                return;
            }
            bulletTimeValues.InBulletTime();

            this.startPoint = startPoint;
            this.ray = ray;
            distanceFromTarget = Vector3.Distance(ray.point, startPoint);
           
            InstantiateObjects();
            GetScriptsFromInstances();
            AssignCameraLookAt();
            CameraPriority();
           
            bulletTargetDistance = Vector3.Distance(ray.point, bulletTimeInstance.transform.position);
            // Scaling time is require before applying force to the bullet
            // otherwise your bullet will move before the time scale.
            enabled = true;
            TimeScaleManager();
            ApplyForce();
        }

        /// <summary>
        /// Instanciating the bullet, the cameras assigned to the bullet and the target.
        /// </summary>
        void InstantiateObjects()
        {
            print("rotation : " + bulletRotation.transform.rotation.y);
            bulletTimeInstance = Instantiate(BulletTimePrefab, startPoint , Quaternion.identity);
            //Debug.Log("Bullet look at target" + ray.point);

            //bulletTimeInstance.transform.DOLookAt(ray.point, 0.05f);

            bulletTimeInstance.GetComponent<BulletCollision>().bulletTarget = ray.transform.root.gameObject;
        
            bulletTimeCameraInstance = Instantiate(BulletTimeCameraPrefab, startPoint, Quaternion.identity);

            bulletTimeCameraInstance.transform.LookAt(ray.point, Vector3.up);


            //targetCameraInstance = Instantiate(TargetCameraPrefab, ray.point, Quaternion.identity);
        }

        /// <summary>
        /// Get all scripts from instantied objects.
        /// </summary>
        private void GetScriptsFromInstances()
        {
            print("Shoot");
            bulletVirtualCameraInstance = bulletTimeCameraInstance.GetComponentInChildren<CinemachineVirtualCamera>();

            bulletDollyCartInstance = bulletTimeCameraInstance.GetComponentInChildren<CinemachineDollyCart>();
           
        }

        void AssignCameraLookAt()
        {
            bulletVirtualCameraInstance.LookAt = bulletTimeInstance.transform;
            bulletVirtualCameraInstance.Follow = bulletTimeInstance.transform; 
        }

        private void Update()
        {
            //print("rotation : " + bulletRotation.transform.rotation.y);

            key = Vector3.Distance(startPoint, bulletTimeInstance.transform.position) / distanceFromTarget;

            //Debug.Log("Key" + key);

            if (key > 0.9f && !targetHited)
            {
                targetHited = true;
                StartCoroutine(nameof(EndBulletTime));
            }


            if(key < 1 && key > 0.97) //added by smit
            {
                ray.transform.GetComponent<AnimalHit>().CameraHit.gameObject.SetActive(true); 
                Destroy(bulletTimeCameraInstance);

            }

            if (key < 0.9) //added by smit
            {
                CameraFollowBullet();
            }

            CameraDutch();
            LookAtTargetPosition();
            CameraPriority();
        }


        private void FixedUpdate()
        {
            CameraPosition();
        }

        private void OnDisable()
        {
            targetHited = false;
            TimeScaleManager();
           
            simpleRifleController.HitMissController(bulletTimeInstance.GetComponent<BulletCollision>().didHitTarget, ray);
            //Destroy(bulletTimeInstance);
            //Destroy(bulletTimeCameraInstance);
            Destroy(targetCameraInstance);
            bulletTimeValues.OffBulletTime();
        }

        /// <summary>
        /// Scale the time according to the time curve.
        /// </summary>
        void TimeScaleManager()
        {
            Time.timeScale = enabled ? bulletTimeValues.TimeScale : 1f;
            Time.fixedDeltaTime = bulletTimeValues.fixedDeltaTime * Time.timeScale;
        }

        IEnumerator EndBulletTime()
        {
            yield return new WaitForSecondsRealtime(bulletTimeValues.deathTime);
            enabled = false;
        }

        void ApplyForce()
        {
            bulletTimeInstance.TryGetComponent(out Rigidbody bulletRigidBody);
            Vector3 direction = (ray.point - bulletTimeInstance.transform.position).normalized;
            bulletRigidBody.AddForce(bulletTimeValues.bulletSpeedBulletMotion * bulletRigidBody.mass * direction);
           
            bulletTimeInstance.GetComponent<BulletCollision>().targetRagdoll = ray.transform.root.GetComponent<RagdollOnOff>();
            bulletTimeInstance.GetComponent<BulletCollision>().targetTransformPosition= ray.point;
            bulletTimeInstance.GetComponent<BulletCollision>().rbTarget = ray.transform.GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Positioning the LookAt target between the bullet and the target
        /// at a position according to the CameraLookAt curve
        /// </summary>
        void LookAtTargetPosition()
        {
            var bulletTargetDirection = (ray.point - bulletTimeInstance.transform.position).normalized;
            bulletTargetDistance = Vector3.Distance(ray.point, bulletTimeInstance.transform.position);
            LookAtTarget.position = 
                bulletTimeInstance.transform.position 
                + bulletTargetDistance 
                * bulletTimeValues.CameraLookAtCurve.Evaluate(key) 
                * bulletTargetDirection;
        }

        void CameraDutch()
        {
            var bulletValue = bulletTimeValues.BulletCameraDutchCurve.Evaluate(key);
            bulletVirtualCameraInstance.m_Lens.Dutch = bulletValue;
            //targetVirtualCameraInstance.m_Lens.Dutch = 
            //bulletTimeValues.UseBulletDutchCurveForBoth ? bulletValue : bulletTimeValues.TargetCameraDutchCurve.Evaluate(key);
        }

        void CameraFollowBullet()
        {
            bulletTimeCameraInstance.transform.position = bulletTimeInstance.transform.position;
            //bulletTimeCameraInstance.transform.position = Vector3.Lerp(bulletTimeCameraInstance.transform.position, bulletTimeInstance.transform.position, 10 * Time.deltaTime);
            //bulletTimeCameraInstance.transform.parent = bulletTimeInstance.transform;

        }

        /// <summary>
        /// Positioning the cart ont the track according to the cameras position curves.
        /// </summary>
        void CameraPosition()
        {
            bulletDollyCartInstance.m_Position = bulletTimeValues.BulletCameraPositionCurve.Evaluate(key);
            //targetDollyCartInstance.m_Position = bulletTimeValues.TargetCameraPositionCurve.Evaluate(key);
        }

        /// <summary>
        /// Set the camera priority according to the priority curve.
        /// </summary>
        void CameraPriority()
        {
            if (bulletTimeValues.CameraPriorityCurve.Evaluate(key) == 0)
            {
                bulletVirtualCameraInstance.Priority = bulletTimeValues.cameraPripority + 1;
            }
            else if (bulletTimeValues.CameraPriorityCurve.Evaluate(key) == 1)
            {
                bulletVirtualCameraInstance.Priority = bulletTimeValues.cameraPripority - 1;
            }
        }        
    }
}