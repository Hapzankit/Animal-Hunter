using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLevel", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public int LevelNo;

    public string AnimalToShoot;

    public bool isCleard;

}
