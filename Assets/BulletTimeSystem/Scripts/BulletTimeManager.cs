using Cinemachine;
using HapzsoftGames;
using System.Collections;
using UnityEngine;

namespace TheBox
{
    public class BulletTimeManager : MonoBehaviour
    {
        public static BulletTimeManager instance;

        [Header("Bullet time Scriptable Objects")]
        [SerializeField] BulletTimeScriptableObject bulletTimeValues = default;

        [Header("Prefabs")]
        [SerializeField] GameObject BulletTimePrefab = default;
        [SerializeField] GameObject BulletTimeCameraPrefab = default;
        //[SerializeField] GameObject TargetCameraPrefab = default;

        [Header("Camera LookAt")]
        [SerializeField] Transform LookAtTarget = default;
                
        /// <summary>
        /// We keep the instance of the prefabs during all the process and destroy it at the end.
        /// </summary>
        GameObject bulletTimeInstance = default;
        GameObject bulletTimeCameraInstance = default;
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
            enabled = false;
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
            AssigneCameraLookAt();
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
            bulletTimeInstance.transform.LookAt(ray.point, Vector3.up);

            
            bulletTimeCameraInstance = Instantiate(BulletTimeCameraPrefab, startPoint, Quaternion.identity);
            
            //bulletTimeCameraInstance.GetComponent<BulletTimeEffectController>().hitRay = ray;  //added by smit
            //bulletTimeCameraInstance.GetComponent<BulletTimeEffectController>().isInBulletTime = true;  //added by smit

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

            //targetVirtualCameraInstance = targetCameraInstance.GetComponentInChildren<CinemachineVirtualCamera>();

            bulletDollyCartInstance = bulletTimeCameraInstance.GetComponentInChildren<CinemachineDollyCart>();
            //targetDollyCartInstance = targetCameraInstance.GetComponentInChildren<CinemachineDollyCart>();
        }

        void AssigneCameraLookAt()
        {
            //bulletVirtualCameraInstance.LookAt = LookAtTarget;
            bulletVirtualCameraInstance.LookAt = bulletTimeInstance.transform;
            bulletVirtualCameraInstance.Follow = bulletTimeInstance.transform;
         
            //targetVirtualCameraInstance.LookAt = LookAtTarget;
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


            if(key < 1 && key > 0.97)        //added by smit
            {
                ray.transform.GetComponent<AnimalHit>().CameraHit.gameObject.SetActive(true); 
                Destroy(bulletTimeCameraInstance);

            }

            if (key <0.9)        //added by smit
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
            Destroy(bulletTimeInstance);
           // Destroy(bulletTimeCameraInstance);
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
            bulletRigidBody.AddForce(
                bulletTimeValues.bulletSpeed * bulletRigidBody.mass * bulletTimeInstance.transform.forward);
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
            //    bulletTimeValues.UseBulletDutchCurveForBoth ? bulletValue : bulletTimeValues.TargetCameraDutchCurve.Evaluate(key);
        }

        void CameraFollowBullet()
        {
            bulletTimeCameraInstance.transform.position = bulletTimeInstance.transform.position;
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