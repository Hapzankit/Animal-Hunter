using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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

        public AnimalPartPosition partPosition;

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
            animal.isAlive = false;
            //animator.SetFloat("Dead Animation", Random.Range(0, 2));
           // animator.SetBool("IsDead", true);
            //animator.SetBool("IsIdle", false);
            //animator.SetBool("IsWalking", false);

            animal.SetState(AnimalState.Death);
            //animal.ragdollOnOff.RagDollModeOn();

            simpleRifleController.StartCoroutine(simpleRifleController.CameraOn(CameraHit));
            Debug.Log("Animal is dead");
            LevelManager.instance.shootedAnimal = gameObject.tag;
            //LevelManager.instance.LevelClearCheck();

        }
        
        public void TakeDamage(int damage)
        {
            animal.weakPoints.HideweakPoints();

            animal.health -= damage;

            float newHealth = animal.health / 90;
            Debug.Log("The health of the hippo left is " + newHealth);

            animal_health_bar.transform.parent.gameObject.SetActive(true);

            if(animal.health == 30)
            {
                animal_health_bar.transform.parent.LookAt(Camera.main.transform);
            }
            else
            {
                animal_health_bar.transform.parent.LookAt(CameraHit.transform);
            }

            animal_health_bar.DOFillAmount(newHealth, 0.5f).OnComplete(DisableHealthBar); 
                                                                                        
           
        }

        public virtual IEnumerator WaitBeforeAttack()
        {
            yield return new WaitForSeconds(2f);

            animal.currentState = AnimalState.Wounded;
            animal.UpdateState();

        }
        void DisableHealthBar()
        {
           // if(animal.health <= 1)
           // {
                animal_health_bar.transform.parent.gameObject.SetActive(false);
           // }
        }
    }


}

public enum AnimalPartPosition
{
    Front, Mid, Back
}