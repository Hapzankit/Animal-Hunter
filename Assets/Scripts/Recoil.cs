using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    public  float recoilAmount = 5.0f; // Amount of recoil
    public  float recoilSpeed = 5.0f; // Speed at which the recoil moves the gun
    public  float returnSpeed = 2.0f; // Speed at which the gun returns to the original position

    private  Vector3 originalPosition;
    private  Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Return smoothly to original position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * returnSpeed);
    }


    public  void Fire()
    {
        print("Recoid AAya");
        targetPosition += Vector3.back * recoilAmount;
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(recoilSpeed);
        targetPosition = originalPosition;
    }
}
