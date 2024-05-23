using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Scriptable Objects/SOListAnimalStat")]
public class SOListAnimalStat : ScriptableObject
{
    public List<SOAnimaStat> AnimaList = new List<SOAnimaStat>();
}
