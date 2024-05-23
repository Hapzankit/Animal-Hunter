using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public List<Animals> CurrentAinmalInScene = new List<Animals>();

    private void Start()
    {
        LevelManager.instance.animalsCurrentlyLive.Clear();
        
        foreach (Animals animal in CurrentAinmalInScene)
        {
            LevelManager.instance.animalsCurrentlyLive.Add(animal);
        }
    }
}
