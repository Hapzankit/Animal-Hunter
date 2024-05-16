using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public BoxCollider[] maincollider;
    public BoxCollider mainGameobjectcollider;
    public GameObject objectRig;
    public Animator objectAnimator;

    private void Start()
    {
        GetRagDollBits();
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

    Collider[] ragDollCollider;
    Rigidbody[] ragDollRigidbody;

    private void GetRagDollBits()
    {
        ragDollCollider = objectRig.GetComponentsInChildren<Collider>();
        ragDollRigidbody = objectRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagDollModeOn()
    {
        objectAnimator.enabled = false;

        foreach (Collider col in ragDollCollider)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rigid in ragDollRigidbody)
        {
            rigid.isKinematic = false;
        }

        //maincollider[0].enabled = true;
        //maincollider[1].enabled = true;
        //maincollider[2].enabled = true;
        mainGameobjectcollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void RagDollModeOff()
    {
        foreach (Collider col in ragDollCollider)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rigid in ragDollRigidbody)
        {
            rigid.isKinematic = true;
        }

        mainGameobjectcollider.enabled = true;
        //maincollider[0].enabled = true;
        //maincollider[1].enabled = true;
        //maincollider[2].enabled = true;
        objectAnimator.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
