using System.Collections.Generic;
using UnityEngine;

namespace TheBox
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Bullet Time")]
    public class BulletTimeScriptableObject : ScriptableObject
    {
        [Header("Time scale")]
        [Range(0.08f, 1f)]
        public float TimeScale = default;

        [Header("Cart position on the dolly track")]
        [Tooltip("Ex: x:0 and y:0.9 = The cart Start at 90% of the lenght of it's dolly track.")]
        public AnimationCurve BulletCameraPositionCurve = default;
        public AnimationCurve TargetCameraPositionCurve = default;

        [Header("Camera LookAt Bullet/Target")]
        [Tooltip("Camera LookAt curve lets the camera know which object to look at." +
            "The value y:0 represents the bullet, and the value y:1 represents the target.")]
        public AnimationCurve CameraLookAtCurve = default;

        [Header("Camera Dutch")]
        [Tooltip("Camera Z roll or Tilt, in degrees. -180 to 180")]
        public AnimationCurve BulletCameraDutchCurve = default;
        public AnimationCurve TargetCameraDutchCurve = default;
        public bool UseBulletDutchCurveForBoth = default;

        [Header("Cameras priority")]
        [Tooltip("y:0 represents the camera attached to the bullet and y:1 is the camera attached to the target.")]
        public AnimationCurve CameraPriorityCurve = default;

        [Space]
        [Tooltip("Death time is the number of seconds you want to wait, after the bullet hits the target, before giving back the focus"
                  + "to the player.")]
        public float deathTime = 3f;

        [Space]
        [Tooltip("Force applied to the bullet rigidbody.")]
        public float bulletSpeed = 33333f;

        [Space]
        [Tooltip("Priority between both camera.")]
        public int cameraPripority = 1500;

        [HideInInspector] public float fixedDeltaTime = default;

        private void Awake()
        {
            fixedDeltaTime = Time.fixedDeltaTime;
        }
        
        /// <summary>
        /// INTERFACE
        /// </summary>

        List<IBulletTime> bulletTimes = new();
        public void RegisterListener(IBulletTime listener) => bulletTimes.Add(listener);

        public void UnregisterListener(IBulletTime listener) => bulletTimes.Remove(listener);

        public void InBulletTime()
        {
            for (int i = 0; i < bulletTimes.Count; i++) bulletTimes[i].InBulletTime();
        }

        public void OffBulletTime()
        {
            for (int i = 0; i < bulletTimes.Count; i++) bulletTimes[i].OffBulletTime();
        }
    }

    public interface IBulletTime
    {
        void InBulletTime();
        void OffBulletTime();
    }
}
