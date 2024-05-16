using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lycaon : MonoBehaviour
{
    private Animator lycaon;
    private bool Speed1 = true;
    private bool Speed2 = false;
    private bool Speed3 = false;
    private bool Speed4 = false;
    private bool Speed5 = false;

    void Start()
    {
        lycaon = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Speed1 = !Speed1;
            Speed2 = false;
            Speed3 = false;
            Speed4 = false;
            Speed5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Speed2 = !Speed2;
            Speed1 = false;
            Speed3 = false;
            Speed4 = false;
            Speed5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Speed3 = !Speed3;
            Speed1 = false;
            Speed2 = false;
            Speed4 = false;
            Speed5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Speed4 = !Speed4;
            Speed1 = false;
            Speed2 = false;
            Speed3 = false;
            Speed5 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Speed5 = !Speed5;
            Speed1 = false;
            Speed2 = false;
            Speed3 = false;
            Speed4 = false;
        }
        if (lycaon.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            lycaon.SetBool("attackleft", false);
            lycaon.SetBool("attackright", false);
            lycaon.SetBool("walk", false);
            lycaon.SetBool("trot", false);
            lycaon.SetBool("run", false);
            lycaon.SetBool("sneak", false);
            lycaon.SetBool("idletolay", false);
            lycaon.SetBool("laytosleep", false);
            lycaon.SetBool("jump", false);
            lycaon.SetBool("jumpinplace", false);
            lycaon.SetBool("sleeptoidle", false);
            lycaon.SetBool("eat", false);
            lycaon.SetBool("drink", false);
            lycaon.SetBool("scratch", false);
            lycaon.SetBool("hitleft", false);
            lycaon.SetBool("hitright", false);
        }
        if (lycaon.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("attackleft", false);
            lycaon.SetBool("attackright", false);
        }
        if (lycaon.GetCurrentAnimatorStateInfo(0).IsName("lay"))
        {
            lycaon.SetBool("laytosleep", false);
            lycaon.SetBool("sleeptoidle", false);
        }
        if (lycaon.GetCurrentAnimatorStateInfo(0).IsName("sleep"))
        {
            lycaon.SetBool("sleeptoidle", false);
            lycaon.SetBool("idletolay", false);
        }
        //---------------------------------
        if ((Input.GetKeyDown(KeyCode.W)) && (Speed1 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("walk", true);
        }
        if ((Input.GetKeyUp(KeyCode.W)) && (Speed1 == true))
        {
            lycaon.SetBool("walk", false);
            lycaon.SetBool("idle", true);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed1 == true))
        {
            lycaon.SetBool("walkleft", true);
            lycaon.SetBool("walk", false);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed1 == true))
        {
            lycaon.SetBool("walk", true);
            lycaon.SetBool("walkleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed1 == true))
        {
            lycaon.SetBool("walkright", true);
            lycaon.SetBool("walk", false);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed1 == true))
        {
            lycaon.SetBool("walk", true);
            lycaon.SetBool("walkright", false);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed1 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("turnleft", true);
        }
        if ((Input.GetKeyUp(KeyCode.A))&& (Speed1 == true))
        {
            lycaon.SetBool("idle", true);
            lycaon.SetBool("turnleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed1 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("turnright", true);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed1 == true))
        {
            lycaon.SetBool("idle", true);
            lycaon.SetBool("turnright", false);
        }
        if ((Input.GetMouseButton(0)) && (Speed1 == true))
        {
            lycaon.SetBool("eat", true);
            lycaon.SetBool("idle", false);
        }
        if ((Input.GetMouseButton(1)) && (Speed1 == true))
        {
            lycaon.SetBool("drink", true);
            lycaon.SetBool("idle", false);
        }
        if ((Input.GetMouseButton(2)) && (Speed1 == true))
        {
            lycaon.SetBool("scratch", true);
            lycaon.SetBool("idle", false);
        }
        //----------------------------------
        if ((Input.GetKeyDown(KeyCode.W)) && (Speed2 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("trot", true);
        }
        if ((Input.GetKeyUp(KeyCode.W)) && (Speed2 == true))
        {
            lycaon.SetBool("trot", false);
            lycaon.SetBool("idle", true);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed2 == true))
        {
            lycaon.SetBool("trotleft", true);
            lycaon.SetBool("trot", false);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed2 == true))
        {
            lycaon.SetBool("trot", true);
            lycaon.SetBool("trotleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed2 == true))
        {
            lycaon.SetBool("trotright", true);
            lycaon.SetBool("trot", false);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed2 == true))
        {
            lycaon.SetBool("trot", true);
            lycaon.SetBool("trotright", false);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed2 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("turnleft", true);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed2 == true))
        {
            lycaon.SetBool("idle", true);
            lycaon.SetBool("turnleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed2 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("turnright", true);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed2 == true))
        {
            lycaon.SetBool("idle", true);
            lycaon.SetBool("turnright", false);
        }
        //---------------------------
        if ((Input.GetKeyDown(KeyCode.W)) && (Speed3 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("run", true);
        }
        if ((Input.GetKeyUp(KeyCode.W)) && (Speed3 == true))
        {
            lycaon.SetBool("run", false);
            lycaon.SetBool("idle", true);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed3 == true))
        {
            lycaon.SetBool("runleft", true);
            lycaon.SetBool("run", false);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed3 == true))
        {
            lycaon.SetBool("run", true);
            lycaon.SetBool("runleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed3 == true))
        {
            lycaon.SetBool("runright", true);
            lycaon.SetBool("run", false);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed3 == true))
        {
            lycaon.SetBool("run", true);
            lycaon.SetBool("runright", false);
        }
        if ((Input.GetMouseButton(0)) && (Speed3 == true))
        {
            lycaon.SetBool("jump", true);
            lycaon.SetBool("run", false);
            lycaon.SetBool("runleft", false);
            lycaon.SetBool("runright", false);
        }
        if ((Input.GetMouseButton(0)) && (Speed3 == true))
        {
            lycaon.SetBool("jumpinplace", true);
            lycaon.SetBool("idle", false);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed3 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("turnleft", true);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed3 == true))
        {
            lycaon.SetBool("idle", true);
            lycaon.SetBool("turnleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed3 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("turnright", true);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed3 == true))
        {
            lycaon.SetBool("idle", true);
            lycaon.SetBool("turnright", false);
        }
        //-------------------------
        if ((Input.GetKeyDown(KeyCode.W)) && (Speed4 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("sneak", true);
        }
        if ((Input.GetKeyUp(KeyCode.W)) && (Speed4 == true))
        {
            lycaon.SetBool("sneak", false);
            lycaon.SetBool("idle", true);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed4 == true))
        {
            lycaon.SetBool("sneakleft", true);
            lycaon.SetBool("sneak", false);
            lycaon.SetBool("idle", false);
            lycaon.SetBool("hitleft", true);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed4 == true))
        {
            lycaon.SetBool("sneak", true);
            lycaon.SetBool("sneakleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed4 == true))
        {
            lycaon.SetBool("sneakright", true);
            lycaon.SetBool("sneak", false);
            lycaon.SetBool("idle", false);
            lycaon.SetBool("hitright", true);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed4 == true))
        {
            lycaon.SetBool("sneak", true);
            lycaon.SetBool("sneakright", false);
        }
        if ((Input.GetMouseButton(0)) && (Speed4 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("attackleft", true);
            lycaon.SetBool("sneak", false);
            lycaon.SetBool("sneakleft", false);
            lycaon.SetBool("sneakright", false);
        }
        if ((Input.GetMouseButton(1)) && (Speed4 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("attackright", true);
            lycaon.SetBool("sneak", false);
            lycaon.SetBool("sneakleft", false);
            lycaon.SetBool("sneakright", false);
        }
        if ((Input.GetMouseButton(2)) && (Speed4 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("dieleft", true);
        }
        //--------------------------------
        if ((Input.GetKeyDown(KeyCode.W)) && (Speed5 == true))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("swim", true);
        }
        if ((Input.GetKeyUp(KeyCode.W)) && (Speed5 == true))
        {
            lycaon.SetBool("swim", false);
            lycaon.SetBool("idle", true);
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (Speed5 == true))
        {
            lycaon.SetBool("swimleft", true);
            lycaon.SetBool("swim", false);
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (Speed5 == true))
        {
            lycaon.SetBool("swim", true);
            lycaon.SetBool("swimleft", false);
        }
        if ((Input.GetKeyDown(KeyCode.D)) && (Speed5 == true))
        {
            lycaon.SetBool("swimright", true);
            lycaon.SetBool("swim", false);
        }
        if ((Input.GetKeyUp(KeyCode.D)) && (Speed5 == true))
        {
            lycaon.SetBool("swim", true);
            lycaon.SetBool("swimright", false);
        }
        //------------------------------
        if (Input.GetKeyDown(KeyCode.S))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("backward", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            lycaon.SetBool("backward", false);
            lycaon.SetBool("idle", true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lycaon.SetBool("idle", false);
            lycaon.SetBool("idletolay", true);
            lycaon.SetBool("laytosleep", true);
            lycaon.SetBool("lay", false);
            lycaon.SetBool("sleep", false);
            lycaon.SetBool("sleeptoidle", true);
        }

    }
}
