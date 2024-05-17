using HapzsoftGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace HapzsoftGames
{
    public class HippoHit : AnimalHit
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                print("Animal got hit : " + gameObject.name);

                //CameraHit.gameObject.SetActive(true);

                //  Destroy(collision.gameObject);
                Vector3 bloodpos= new Vector3(other.transform.position.x - 0.2f, other.transform.position.y, other.transform.position.z);
                blood = Instantiate(BloodPrefab, bloodpos, Quaternion.identity);
                blood.transform.SetParent(hitpos.transform);
                //StartCoroutine(AnimationDelay());

                //Camera.gameObject.SetActive(true);

                SimpleRifleController simpleRifleController = FindObjectOfType<SimpleRifleController>();

                

                switch (gameObject.name)
                {
                    case "Base":
                        // Handle collision with an enemy
                        print(" MID!");
                        
                        TakeDamage(other.GetComponent<BulletCollision>().bulletDamage);

                        if (animal.health > 0)
                        {
                            CheckToSetPlayer("Hippo|Hit_M", simpleRifleController);
                            StartCoroutine(WaitBeforeAttack());
                        }
                        else
                        {
                            CheckToKill(simpleRifleController);
                        }

                        break;
                    case "Root":
                        // Handle collision with a wall
                        print("BACK!");

                        TakeDamage(other.GetComponent<BulletCollision>().bulletDamage);

                        if (animal.health > 0)
                        {
                            CheckToSetPlayer("Hippo|Hit_B", simpleRifleController);
                            StartCoroutine(WaitBeforeAttack());

                        }
                        else
                        {
                            CheckToKill(simpleRifleController);
                        }

                        break;
                    case "Spine_1":
                        // Handle collision with player (if projectiles can hit the player)
                        print("FRONT!");
                        
                        TakeDamage(100);

                        CheckToKill(simpleRifleController);

                        break;
                    default:
                        // Handle any other unspecified tags
                        print("Projectile hit an object with an unhandled tag.");
                        break;
                }

                
                hitdear = true;
                
                other.gameObject.SetActive(false);

            }
        }

        private IEnumerator WaitBeforeAttack()
        {
            yield return new WaitForSeconds(2f);

            animal.currentState = AnimalState.Wounded;
            animal.UpdateState();

        }
    }


   
}