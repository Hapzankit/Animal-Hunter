using Knife.Effects.SimpleController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Source")]
    public AudioSource audioSource;
    [Header("Audio Clip")]
    public AudioClip[] audioClip;
    public AudioClip walkingClip;

    public GameObject target;
    public GameObject target2;

    public Animator animator;

    public SimpleRifleController controller;
    public PlayerMovement playerController;
    public Button[] button;
    public int played;


    void Start()
    {
        //played = PlayerPrefs.GetInt("isPlayed", 0);

        //if (played == 0)
        //{
        //    PlayerPrefs.SetInt("isPlayed", 1);
            playerController.enabled = false;
            // controller.enabled = false;
            button[0].interactable = false;
            button[1].interactable = false;
            StartCoroutine(playSound());
            StartCoroutine(SetANimationForNPC());
        //}

    }

    public IEnumerator playSound()
    {

        yield return new WaitForSeconds(2f);
        audioSource.volume = 1f;
        audioSource.PlayOneShot(audioClip[0]);
        yield return new WaitForSeconds(4f);

        controller.enabled = true;
        playerController.enabled = true;
        button[0].interactable = true;
        button[1].interactable = true;
        audioSource.PlayOneShot(audioClip[1]);

        controller.isCutScene = false;
        yield return new WaitForSeconds(4f);

        audioSource.PlayOneShot(audioClip[2]);

        yield return new WaitForSeconds(4f);

        audioSource.PlayOneShot(audioClip[3]);

        yield return new WaitForSeconds(4f);

        audioSource.PlayOneShot(audioClip[4]);
    }

    IEnumerator SetANimationForNPC()
    {
        animator.SetBool("IsMoving", true);
        InvokeRepeating("playWalkingSoundForGuide", 0, 1.5f);
        yield return new WaitForSeconds(1.5f);

        animator.SetBool("IsMoving", false);
        CancelInvoke();
    }

    public void playWalkingSoundForGuide()
    {
        print("walk");
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(walkingClip);
    }
}
