using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Reference to the CameraController script
    //public CameraController cameraController;
    public GameObject bulletRotation;

    public int bulletDamage = 30;

    public bool didHitTarget;

    [HideInInspector]
    public bool TargetHitCamera;

    private Rigidbody rb;

    
    public Rigidbody rbTarget;
    public Vector3 targetTransformPosition;
    public RagdollOnOff targetRagdoll;

    public GameObject bulletTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private bool force_Added;
    private int forceAddCounter;

    private void Update()
    {
       transform.LookAt(targetTransformPosition);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        // Access the GameObject that is colliding with the projectile
        if(other != null && LayerMask.LayerToName(other.transform.root.gameObject.layer) == "Animal")
        {
            Debug.Log("The bullet hit the " + other.transform.name);
            
            didHitTarget = true;

            AnimalInterference.instance.CheckInterference(bulletTarget, other.transform.root.gameObject);

            Destroy(rb);
            Destroy(GetComponent<Collider>());
        }
        else
        {
            Debug.Log("Bullet hit the " + other.name);
            
            didHitTarget = false;

        }

    }
}
