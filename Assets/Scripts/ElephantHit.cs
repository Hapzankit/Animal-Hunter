using HapzsoftGames;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



namespace HapzsoftGames
{
    public class ElephantHit : AnimalHit
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                print("Animal got hit : " + gameObject.name);

                //CameraHit.gameObject.SetActive(true);

                //  Destroy(collision.gameObject);
                Vector3 bloodpos = new Vector3(other.transform.position.x - 0.2f, other.transform.position.y, other.transform.position.z);
                blood = Instantiate(BloodPrefab, bloodpos, Quaternion.identity);
                blood.transform.SetParent(hitpos.transform);
                //StartCoroutine(AnimationDelay());

                //Camera.gameObject.SetActive(true);

                SimpleRifleController simpleRifleController = FindObjectOfType<SimpleRifleController>();



                switch (gameObject.name)
                {
                    
                    case "spine1_hiResSpine1":
                        // Handle collision with a wall
                        print("BACK!");

                        TakeDamage(other.GetComponent<BulletCollision>().bulletDamage);

                        if (Dear.GetComponentInParent<Elephant>().health > 0)
                        {
                            CheckToSetPlayer("Hit BAck", simpleRifleController);
                            StartCoroutine(WaitBeforeAttack());
                        }
                        else
                        {
                            CheckToKill(simpleRifleController);
                        }                   

                        break;

                    case "spine1_hiResSpine5":

                        TakeDamage(100);

                        // Handle collision with player (if projectiles can hit the player)
                        print("FRONT!");
                        CheckToKill(simpleRifleController);
                        break;

                    default:
                        // Handle any other unspecified tags
                        print("Projectile hit an object with an unhandled tag.");
                        break;
                }

                //animator.SetBool("IsDead", true);
                hitdear = true;
                //StartCoroutine(TurnObjectOff());
                other.gameObject.SetActive(false);

            }
        }
    }
}