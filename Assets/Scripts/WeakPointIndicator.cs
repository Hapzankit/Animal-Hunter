using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeakPointIndicator : MonoBehaviour
{
    public GameObject weakPoint; 
    public RectTransform uiElement; 
    public Camera scopeCamera;
    private WeakPoints lastHitAnimals;
    private SimpleRifleController riflecontroller;

    public void Start()
    {
        riflecontroller = GetComponent<SimpleRifleController>();
    }


    private void FixedUpdate()
    {
       
        RaycastHit hit;

        if (Camera.main != null && riflecontroller.isScopedIn)
        {

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            {
                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Animal")
                {
                    WeakPoints hitanimal = hit.transform.root.GetComponent<WeakPoints>();
                    if (hitanimal != lastHitAnimals)
                    {
                        hitanimal.ShowWeakPoint();
                        if (lastHitAnimals != null)
                        {
                            lastHitAnimals.HideweakPoints();
                        }
                    }

                    lastHitAnimals = hitanimal;
                }
                else
                {
                    if (lastHitAnimals != null)
                    {
                        lastHitAnimals.HideweakPoints();
                        lastHitAnimals = null;
                    }
                }
            }
           
        }
    }
}

