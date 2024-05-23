using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToPlayer : MonoBehaviour
{
    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.LookAt(player.transform);
    }
}
