using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new gun", menuName = "Gun")]
public class GunType : ScriptableObject
{
    public GameObject gun;
    public GameObject bullet;

    public int maxAmmo;
    public float rechargeTime;
    public float maxRecoil;
}
