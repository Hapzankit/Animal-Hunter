using FirstPersonMobileTools.DynamicFirstPerson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

namespace HapzsoftGames
{
    public abstract class AnimalHit : MonoBehaviour
    {
        public GameObject Dear;
        public GameObject BloodPrefab;
        public Camera CameraHit;
        public Camera Camera;
        public bool hitdear;
        public GameObject hitpos;
        public GameObject blood;
        public Animals animal;

        public float stopFollowingDistance = 5f; // Distance at which camera stops following
        public GameObject projectile; // Reference to the projectile

        public Animator animator;

        [SerializeField]
        Image animal_health_bar;


        private void Start()
        {
            animal = transform.root.GetComponent<Animals>();
        }



        IEnumerator TurnObjectOff()
        {
            yield return new WaitForSeconds(1f);
            Dear.GetComponent<Hippo>().enabled = false;
        }

        IEnumerator AnimationDelay()
        {
            yield return new WaitForSeconds(0.1f);
            //animator.Play("Hippo|Hit_M");
            yield return new WaitForSeconds(0.1f);
            print("Deaddddddddd");
            animator.SetBool("IsDead", true);
            //blood.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            //CameraHit.gameObject.SetActive(false);
            Camera.gameObject.SetActive(true);

        }



        public virtual void CheckToSetPlayer(string animationName, SimpleRifleController simpleRifleController)
        {
            animator.Play(animationName);
            simpleRifleController.StartCoroutine(simpleRifleController.SetPlayerController(CameraHit));
        }

        public virtual void CheckToKill(SimpleRifleController simpleRifleController)
        {

            animator.SetBool("IsDead", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            simpleRifleController.StartCoroutine(simpleRifleController.CameraOn());
            Debug.Log("Animal is dead");
            LevelManager.instance.shootedAnimal = gameObject.tag;
            LevelManager.instance.LevelClearCheck();

        }
        
        public void TakeDamage(int damage)
        {
            animal.health -= damage;

            float newHealth = animal.health / 90;
            Debug.Log("The health of the hippo left is " + newHealth);

            animal_health_bar.fillAmount = newHealth; //Here later on instead of 100 that totla health animal will come 
                                                                                        //Currently used 100 because all animals have 100 as a total health.
        }
    }


}
