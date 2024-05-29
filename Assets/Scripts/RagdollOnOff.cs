using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    [SerializeField] 
    BoxCollider[] maincollider;
    
    [SerializeField] 
    Animator objectAnimator;

    [SerializeField] 
    Collider[] ragDollCollider;

    [SerializeField]
    Rigidbody[] ragDollRigidbody;

    private void Start()
    {
        //GetRagDollBits();
        RagDollModeOff();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            print("Hittttt");
            RagDollModeOn();
        }
    }

    public void RagDollModeOn()
    {
        objectAnimator.enabled = false;

        foreach (Collider col in ragDollCollider) {col.enabled = true;}

        foreach (Rigidbody rigid in ragDollRigidbody) {rigid.isKinematic = false;}

        foreach (Collider col in maincollider) { col.enabled = false; }     
    }

    public void RagDollModeOff()
    {
        foreach (Collider col in ragDollCollider) {col.enabled = false;}

        foreach (Rigidbody rigid in ragDollRigidbody) {rigid.isKinematic = true;}
        
        foreach (Collider col in maincollider) { col.enabled = true; }
        
        objectAnimator.enabled = true;
       
    }
}
