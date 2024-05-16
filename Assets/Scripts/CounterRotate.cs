using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    public Vector3 Rotation;
    void Update()
    {
        transform.rotation = Quaternion.Euler(Rotation); // Keeps the child object's rotation constant.
    }
}
