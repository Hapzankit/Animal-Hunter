using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoints : MonoBehaviour
{
    [SerializeField]
    GameObject[] weakpoints;

    public void ShowWeakPoint()
    {
        StartCoroutine(EnableWeakPoints());
    }

    public void HideweakPoints()
    {
        StopAllCoroutines();
        foreach (GameObject weakPoint in weakpoints)
        {
            weakPoint.SetActive(false);
        }
    }

    IEnumerator EnableWeakPoints()
    {
        foreach (GameObject weakPoint in weakpoints)
        {
            weakPoint.SetActive(!weakPoint.activeInHierarchy);
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(EnableWeakPoints());
    }
}
