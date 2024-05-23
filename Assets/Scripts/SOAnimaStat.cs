using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimaStat", menuName = "Scriptable Objects/SOAnimaStat")]
public class SOAnimaStat : ScriptableObject
{
    public string animalName;
    public Sprite animalImage;
    public string animalSpeed;
    public string animalAge;
    public string animalWeight;
    public string animalHeight;
    public string animalFacts;
}
