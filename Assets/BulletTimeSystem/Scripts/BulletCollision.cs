using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Reference to the CameraController script
    //public CameraController cameraController;
    public GameObject bulletRotation;

    public int bulletDamage = 30;

    private void OnTriggerEnter(Collider other)
    {
        // Access the GameObject that is colliding with the projectile
        if(other != null && LayerMask.LayerToName(other.transform.root.gameObject.layer) == "Animal")
        {

            Debug.Log("The bullet hit the " + other.transform.name);

            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);            
        }
    }
}
