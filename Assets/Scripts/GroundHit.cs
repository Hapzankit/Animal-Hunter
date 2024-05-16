using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHit : MonoBehaviour
{
    public GameObject groungHitEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameObject groungHitEffects = Instantiate(groungHitEffect, new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z), Quaternion.identity);
            groungHitEffects.transform.SetParent(transform);
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GameObject groungHitEffects = Instantiate(groungHitEffect, collision.transform.position, Quaternion.identity);
            groungHitEffects.transform.SetParent(transform);
        }
    }
}
