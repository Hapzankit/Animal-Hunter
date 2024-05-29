using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class SOWeapons : ScriptableObject
{
    public int WeaponAmmoCapacity;
    public int WeaponDamage;
    public int FireRate;
}
