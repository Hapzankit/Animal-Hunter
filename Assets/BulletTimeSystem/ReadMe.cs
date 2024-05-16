
using UnityEngine;

namespace TheBox
{
    public class ReadMe : MonoBehaviour
    {
        /// <summary>
        /// Unity Bullet time FREE SCRIPT.
        /// </summary>
        /// 
        /*
         * How to set up a bullet time system in your game.
         *  
         * Drag and drop the bullet time manager prefab to your hierarchy.
         * 
         * You must have cinemachine package installed to use this asset.
         * 
         * Now, go to the function where you instantiate a projectile or perform a raycast.
         * Call the function:
         * TheBox.BulletTimeManager.instance.LaunchBulletTime("starting position of your bullet", "Your raycast");
         * 
         * with the starting position of your bullet, and the raycast performed on the target you want to hit.
         *
         * From this point , you should be able to perform a Bullet Time on any object that contains a Collider and a Rigidbody.
         *
         * The Bullet Time scenario is configurable by the Bullet time scriptable object.
         *
         * The horizontal axis X, of each animation curve, is the distance between the player and the bullet.
         * 0 represents the starting point of the bullet and 1 the position of the target.
         * 
         * Press F to center the curve.
         * 
         * Each camera has a path it can follow. The camera is parented to a cart and the cart follows a track.
         * The position of the cart on the track, is determined by the Camera's position curve.
         *
         * The Camera LookAt curve lets the camera know which object to look at.
         * The value 0 represents the bullet, and the value 1 represents the target.
         * 
         * You can activate the sphere in the Bullet Time Manager prefab in your hierarchy to see where the camera looking.
         * 
         * Camera dutch curve manages the rotation of your cameras in degres, you can set "bullet dutch curve for both"
         * to true, if you want a single manage both cameras.
         * 
         * The last curve defines the priority of the cameras. 
         * 0 represents the camera attached to the bullet and 1 is the camera attached to the target.
         * 
         * Death time is the number of seconds you want to wait, after the bullet hits the target, before giving back the focus
         * to the player.
         * 
         * Bullet speed is the force applied to the bullet.
         * 
         * The priority curve will add or substract to this value to manage the priority between both camera.
         * 
         * The scriptable object come with an interface. 
         * public interface IBulletTime
         *  {
         *      void InBulletTime();
         *      void OffBulletTime();
         *  }
         * 
         */

    }
}
