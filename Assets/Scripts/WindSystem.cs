using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindSystem : MonoBehaviour
{
    public Vector3 windDirection = Vector3.right;
    public float windForce = 2f;
    public float changeInterval = 5f;

    private float timer;

    void Start()
    {
        ChangeWindDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            ChangeWindDirection();
            timer = 0f;
        }
    }

    void ChangeWindDirection()
    {
        // Change wind direction randomly, you can customize this logic as needed
        windDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public Vector3 GetWindForce()
    {
        return windDirection * windForce;
    }
}

