// 2017 devotid Assets ^^^^^^^^^^^^^^ Simple Rifle Controller Script ^^^^^^^^^^^^^^^^^^
// This script is not intended to be used as a complete "gun controller" it is only meant to show the model is easily animated and ready for your specific application. :)

using FirstPersonMobileTools.DynamicFirstPerson;
using HapzsoftGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FirstPersonMobileTools.Utility;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using Cinemachine;
using TheBox;


public class SimpleRifleController : MonoBehaviour
{

    [Header("Animator")]
    [SerializeField]
    Animator m_Animator;  //Fetches the Animator attached to this game object in the start function
    [Header("Integer")]
    [SerializeField]
    public int ReloadAmount = 3; //This is the amount of ammo to reload
    public  Camera mainCamera;
    public int CurrentAmountOfAmmo = 1; //Current amount of Ammo in the Rifle
    private int WeaponAmmoCapacity;
    [SerializeField] private float scopeFOVChangeSpeed;
    [SerializeField] private float minScopeFOV;
    [SerializeField] private float maxScopeFOV;
    [SerializeField] private float shootingForce;
    [SerializeField] private float minMouseSensivity;
    [SerializeField] private float maxMouseSensivity;
    [SerializeField] private float minDistanceToPlayAnimation;
    [SerializeField] private float speed = 10;
    [SerializeField] private Camera cutSceneCamera;
    [SerializeField] GameObject playerForCutScene;
    [SerializeField] GameObject mainPlayer;
    [SerializeField] GameObject fpsController;
    [SerializeField] Canvas shootingCanvas;
    [SerializeField] AudioManager audioManager;
    [Header("Bool")]
    public bool isScopedIn;
    [Header("GameObject")]
    public GameObject enableScope;
    public Transform bulletSpawnTransform;
    public Transform SecondbulletSpawnTransform;
    public GameObject demoBullet; // Assign in inspector
    [Header("Vectors")]
    public Vector3 upRecoil;
    [Header("Camera")]
    public Camera scopeCamera;
    public Scrollbar scopeScrol;
    public GameObject Player;
    public GameObject Ui;
    public GameObject GameOver;
    public GameObject ScopeInBtn;
    public GameObject ScopeOutBtn;
    public GameObject ScopeOverlay;
    public GameObject Guide;
    public MovementController MovementController;
    public bool BobSet;
    public Camera firstBulletEffectCam;
    public Camera BulletEffectCam;
    public float bulletSpeed = 3f;
    public GameObject SecondGun;
    public AudioSource BulletAudioSource;
    public AudioClip BulletAudioClip;
    private bool isFired;
    public SOWeapons SOWeapon;
    GameObject Shootedbullet;
    [SerializeField] GameObject mainBullet;

    [SerializeField] LevelTimer levelTimer;


    void Start()
    {
        m_Animator.Play("Start");
        MovementController = FindObjectOfType<MovementController>();
        //WeaponAmmoCapacity = CurrentAmountOfAmmo;
        CurrentAmountOfAmmo = SOWeapon.WeaponAmmoCapacity;
        ReloadAmount = SOWeapon.ReloadAmount;
        UIManager.instance.ShowRemainingBullets(CurrentAmountOfAmmo, SOWeapon.WeaponAmmoCapacity, ReloadAmount, SOWeapon.ReloadAmount);
    }

    public bool isCutScene = true;

    bool calledOnce;

    void Update()
    {
        if(audioManager.played == 0 && !calledOnce)
        {
            if (isCutScene)
            {
                cutSceneCamera.GetComponent<Camera>().enabled = true;
                mainCamera.GetComponent<Camera>().enabled = false;
                playerForCutScene.gameObject.SetActive(true);
                mainPlayer.gameObject.SetActive(false);
                //Debug.Log("Setting UI in Game scene 2");
                shootingCanvas.gameObject.SetActive(false);

                Guide.SetActive(true);
            }
            else 
            { 
                cutSceneCamera.GetComponent<Camera>().enabled = false;
                mainCamera.GetComponent<Camera>().enabled = true;
                playerForCutScene.gameObject.SetActive(false);
                mainPlayer.gameObject.SetActive(true);
                shootingCanvas.gameObject.SetActive(true);
                levelTimer.timerIsRunning = true;
                // Guide.SetActive(false);
                Debug.Log("Called The Ui ");

                calledOnce = true;

            }
        }
        // for cutscene
        
        //else
        //{
        //    cutSceneCamera.GetComponent<Camera>().enabled = false;
        //    mainCamera.GetComponent<Camera>().enabled = true;
        //    playerForCutScene.gameObject.SetActive(false);
        //    mainPlayer.gameObject.SetActive(true);
        //    shootingCanvas.gameObject.SetActive(true);
        //    Guide.SetActive(false);
        //}




        // Check the device orientation and adjust the screen orientation accordingly
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }
        HandleScope();
        scopeCamera.fieldOfView = minScopeFOV + (scopeScrol.value * (maxScopeFOV - minScopeFOV));

        SecondGun.transform.position = Player.transform.position;
        SecondGun.transform.rotation = Player.transform.rotation;
    }

    #region Shooting
    public void FireRifle()
    {


        if (isScopedIn)
        {
            //m_Animator.SetTrigger("ScopeInFIre");
            ToggleScope();
            isScopedIn = false;
            //ScopeInBtn.gameObject.SetActive(true);
           
            //scopeScrol.gameObject.SetActive(false);
            //StartCoroutine(AnimationScopeOut());
        }
        else
        {
            m_Animator.SetTrigger("Fire Rifle"); //Sends a trigger to the Animator controller to show the fire animation once
            enableScope.SetActive(false);
        }


        ReduceAmmo();
        
        Invoke(nameof(CheckIfOutOfBullet), 2);

        Debug.Log("Shot " + CurrentAmountOfAmmo + " fired");
        EventManager.GunshotFired();
    }

    public void Shoot()
    {
        // Cast a ray from the center of the camera to the forward direction
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if ray hits something (optional)
        if (Physics.Raycast(ray, out hit, 200f) && CurrentAmountOfAmmo > 0)
        {

            Debug.DrawRay(ray.origin, Camera.main.transform.forward * 100f, Color.red, Mathf.Infinity);
            
           
            
            if ( LayerMask.LayerToName(hit.collider.gameObject.layer) == "Animal" && !isFired)      
            {
                Debug.Log("Hit Animal" + hit.transform.root.name + " hit body part " + hit.transform.name);

                //Player.SetActive(false);
                //Ui.SetActive(false);
                BulletAudioSource.PlayOneShot(BulletAudioClip);

                // StartCoroutine(FireEffecTrue(hit));
                CheckToShoot(hit);


                hit.collider.GetComponent<AnimalHit>().Dear.GetComponent<Animals>().StopAllCoroutines();
                hit.collider.GetComponent<AnimalHit>().Dear.GetComponent<Animals>().SetNavMeshAgentSpeed(0);
                

               
                StartCoroutine(DelayedFire());
                //StartCoroutine(CameraOn());
                hit.collider.GetComponent<AnimalHit>().Dear.GetComponent<Animator>().SetBool("IsIdle", true);
                hit.collider.GetComponent<AnimalHit>().Dear.GetComponent<Animator>().SetBool("IsWalking", false);
                hit.collider.GetComponent<AnimalHit>().Dear.GetComponent<Animator>().SetBool("IsRunning", false);

            }
            else
            {
                if (!isFired)
                {
                    isFired = true;
                    Debug.Log("Hit " + hit.collider.name); // Log the name of the hit object
                    BulletAudioSource.PlayOneShot(BulletAudioClip);
                    FireRifle();
                    StartCoroutine(FireEffectGroundHit(hit));
                    GameObject bullet = Instantiate(demoBullet, bulletSpawnTransform.position, Quaternion.identity);
                    // Assuming hit.point is defined somewhere in your context as the target position
                    StartCoroutine(DelayedFire());
                    // Calculate direction towards hit.point
                    Vector3 directionToHitPoint = (hit.point - bullet.transform.position).normalized;                    

                    // Get the Rigidbody component of the bullet
                    Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody>();

                    // Apply force in the direction of the hit point
                    // Adjust the force multiplier (4000 in this case) as needed
                    bulletRigidBody.AddForce(4000 * bulletRigidBody.mass * directionToHitPoint);

                    //CheckToRelaod(3);

                }

            }
        }
        else
        {
            if (!isFired && CurrentAmountOfAmmo > 0)
            {
                BulletAudioSource.PlayOneShot(BulletAudioClip);
                FireRifle();
                StartCoroutine(DelayedFire());
                //CheckToRelaod(2);

            }
        }
    }

    public IEnumerator FireEffect(RaycastHit hit)
    {
        yield return new WaitForSeconds(0);
        //Time.timeScale = 0.1f;
        SecondGun.SetActive(true);
        //firstBulletEffectCam.enabled = true;
        GameObject bullet = Instantiate(demoBullet, SecondbulletSpawnTransform.position, Quaternion.identity);
        Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody>();
        bulletRigidBody.AddForce(
        10 * bulletRigidBody.mass * bulletRigidBody.transform.forward);
        yield return new WaitForSeconds(1.5f);
        bullet.SetActive(false);
        StartCoroutine(BulletMotion(hit));

        Debug.Log("Fired Shot 1");
    }

    public IEnumerator FireEffecTrue(RaycastHit hit)
    {
        yield return new WaitForSeconds(0);
        //Time.timeScale = 0.1f;
        SecondGun.SetActive(true);
        //firstBulletEffectCam.enabled = true;
   
        yield return new WaitForSeconds(0.5f);
        //bullet.SetActive(false);
        StartCoroutine(BulletMotion(hit));

        Debug.Log("Fired Shot 2");

    }

    public IEnumerator FireEffectGroundHit(RaycastHit hit)
    {
        yield return new WaitForSeconds(0);
        bulletSpawnTransform.GetChild(0).gameObject.SetActive(true);
        bulletSpawnTransform.GetChild(1).gameObject.SetActive(true);
        firstBulletEffectCam.enabled = false;
        yield return new WaitForSeconds(1.5f);
        bulletSpawnTransform.GetChild(0).gameObject.SetActive(false);
        bulletSpawnTransform.GetChild(1).gameObject.SetActive(false);

        Debug.Log("Fired Shot 3");

    }

    public IEnumerator BulletMotion(RaycastHit hit)
    {
        yield return new WaitForSeconds(0f);
        firstBulletEffectCam.enabled = false;
        BulletEffectCam.enabled = true;
        shootingCanvas.gameObject.SetActive(false);
        Debug.Log("Setting UI in Game scene 1");
        TheBox.BulletTimeManager.instance.LaunchBulletTime(bulletSpawnTransform.position, hit);

    }
    #endregion

    #region Scoping
    public void ScopeIn() // Called via Scope UI Button
    {
        m_Animator.Play("Scope In");
        enableScope.SetActive(true);
        isScopedIn = true;
        scopeScrol.gameObject.SetActive(true);
        //ScopeInBtn.gameObject.SetActive(false);
        //ScopeOutBtn.gameObject.SetActive(true);

        Debug.Log("Scope In");
    }

    public void ScopeOut() // Called via Scope UI Button
    {
        print("ScopeOut");
        //m_Animator.SetTrigger("ScopeOut");
        m_Animator.Play("Scope Out");
        isScopedIn = false;
        scopeScrol.gameObject.SetActive(false);
        StartCoroutine(AnimationScopeOut());
        //ScopeInBtn.gameObject.SetActive(true);
        //ScopeOutBtn.gameObject.SetActive(false);
        Debug.Log("Scope out");

    }
    public void ToggleScope() //Adding in the OnButtonClick event in inspector
    {
        if(isScopedIn)
        {
            ScopeOut();
        }
        else
        {
            ScopeIn();
        }
    }

    private void HandleScope()
    {
        if (!isScopedIn)
        {
            ResetScopeFOV();
        }
    }

    internal void ResetScopeFOV()
    {
        scopeCamera.fieldOfView = minScopeFOV;
    }
    #endregion

    public IEnumerator CameraOn(Camera animalCamera)
    {
        yield return new WaitForSeconds(5f);
        animalCamera.gameObject.SetActive(false);
        BulletEffectCam.enabled = false;
        Player.SetActive(true);
        shootingCanvas.gameObject.SetActive(true);

        //fpsController.SetActive(true);
        SecondGun.SetActive(false);
        //m_Animator.enabled = true;
        //ScopeOut();

        LevelManager.instance.LevelClearCheck();
        Debug.Log("Restting player");
    }


    public IEnumerator SetPlayerController(Camera animalCamera)
    {
        yield return new WaitForSeconds(0.5f);
        Player.SetActive(true);
        SecondGun.SetActive(false);
        animalCamera.gameObject.SetActive(false);
        Time.timeScale = 1;
        //m_Animator.enabled = true;
        ResetBulletEffectCam();
        ScopeOut();
        CheckIfOutOfBullet();
        shootingCanvas.gameObject.SetActive(true);

        //CheckToRelaod(1);
    }

    void ResetBulletEffectCam()
    {
        BulletEffectCam.enabled = false;
        BulletEffectCam.transform.position = Vector3.zero;
        BulletEffectCam.transform.rotation = Quaternion.identity;
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
        Destroy(LevelManager.instance.gameObject);
    }

    IEnumerator AnimationScopeOut()
    {
        yield return new WaitForSeconds(0.5f);
        enableScope.SetActive(false);
        m_Animator.SetTrigger("ScopeOut");

    }
    IEnumerator AnimationScopeIn()
    {
        yield return new WaitForSeconds(0.5f);
        enableScope.SetActive(false);
        m_Animator.SetTrigger("Fire Rifle");
    }

    IEnumerator DelayedFire()
    {
        yield return new WaitForSeconds(2f);

        isFired = false;
    }



    //This function decides whether to shoot normaly or with Bullet Motion
    void CheckToShoot(RaycastHit hit)
    {
        ReduceAmmo();
        
        bool isFrontPart = hit.collider.GetComponent<AnimalHit>().partPosition == AnimalPartPosition.Front;
        bool isCriticalHealth = hit.transform.root.GetComponent<Animals>().health <= 30;
        bool isFirstHitToAnimal = hit.transform.root.GetComponent<Animals>().health >= 90;

        if (isFrontPart || isCriticalHealth || isFirstHitToAnimal)
        {
            StartCoroutine(FireEffecTrue(hit));
            Player.SetActive(false);
        }
        else
        {
            SpawnBulletAndAddForce(hit);
        }
    }


    void SpawnBulletAndAddForce(RaycastHit hit)
    {
        Shootedbullet = Instantiate(mainBullet, bulletSpawnTransform.position, Quaternion.identity);
        Shootedbullet.transform.LookAt(hit.point, Vector3.up);
        Rigidbody bulletRB = Shootedbullet.GetComponent<Rigidbody>();
        Debug.Log("Shoot Local Bullet");
        BulletTimeScriptableObject bulletTimeSo = TheBox.BulletTimeManager.instance.bulletTimeValues;
        bulletRB.AddForce(bulletTimeSo.bulletSpeedNormalMotion * bulletRB.mass * Shootedbullet.transform.forward);
    }

    void ReduceAmmo()
    {
        CurrentAmountOfAmmo--;
        UIManager.instance.ShowRemainingBullets(CurrentAmountOfAmmo, SOWeapon.WeaponAmmoCapacity, ReloadAmount, SOWeapon.ReloadAmount);
    }
    public void ReloadWeapon()
    {
        if (CurrentAmountOfAmmo < SOWeapon.WeaponAmmoCapacity)
        {
            if (ReloadAmount > 0)
            {
                int ammoToAdd = SOWeapon.WeaponAmmoCapacity - CurrentAmountOfAmmo;
                CurrentAmountOfAmmo +=  ammoToAdd > ReloadAmount ? ReloadAmount : ammoToAdd;
                ReloadAmount -= ammoToAdd;
                UIManager.instance.ShowRemainingBullets(CurrentAmountOfAmmo, SOWeapon.WeaponAmmoCapacity, ReloadAmount, SOWeapon.ReloadAmount);

            }
            else
            {
                UIManager.instance.gameOverScreen.SetActive(true);
                UIManager.instance.AmmoOverText.gameObject.SetActive(true);
            }
        }
    }

    public void CheckIfOutOfBullet()
    {
        if(CurrentAmountOfAmmo <= 0 && ReloadAmount <= 0)
        {
            UIManager.instance.gameOverScreen.SetActive(true);
            UIManager.instance.AmmoOverText.gameObject.SetActive(true);
        }
    }


    public void HitMissController(bool hitStatus, RaycastHit ray)
    {
        if(!hitStatus)
        {
            AnimalHit animalHit;
            Debug.Log("Hit miss called");
            ray.collider.TryGetComponent<AnimalHit>(out animalHit);
            StartCoroutine(SetPlayerController(animalHit.CameraHit));
            EventManager.GunshotFired();
        }
    }
 
}

