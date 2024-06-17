using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalInterference : MonoBehaviour
{
   

    public static AnimalInterference instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void CheckInterference(GameObject RealTarget, GameObject InterferenceTarget)
    {
        Debug.Log("CheckInterference is called");
        if(RealTarget != InterferenceTarget)
        {
            Debug.Log("CheckInterference is passed");

            Destroy(TheBox.BulletTimeManager.instance.bulletTimeCameraInstance);
            TheBox.BulletTimeManager.instance.enabled = false;
        }
    }
}
