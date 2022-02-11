using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public static Shooting instance;
    public Animator playerAnim;

    [SerializeField] private PlayerInput playerInput;

    public Transform gunHolder;
    public Transform cannon;
    public GameObject bullet;
    public GameObject weapon;
    public GunType gun;

    public int ammo;
    public int gunAmmo;
    public int maxAmmo;
    public Text ammoCount;

    public float maxRecoil;
    float recoil;
    public float rechargeTime;
    float rTime = 0;

    public bool isShooting = false;
    public bool hasGun;

    float weight;
    [SerializeField] float animSpeed;

    public void Awake()
    {
        instance = this;

    }

    public void PickGun()
    {
        weapon = gun.gun;

        maxAmmo = gun.maxAmmo;
        maxRecoil = gun.maxRecoil;
        rechargeTime = gun.rechargeTime;
        bullet = gun.bullet;

        var weaponIns = Instantiate(weapon, gunHolder.position, gunHolder.rotation);
        weaponIns.transform.parent = gunHolder.transform;
        cannon = weaponIns.transform.GetChild(1);
    }

    void OnFire()
    {
        isShooting = !isShooting;
    }

    void OnRecharge()
    {
        if (gunAmmo < maxAmmo && ammo > 0 && rTime <= 0 && hasGun == true)
        {
            rTime = rechargeTime;
            int substr = maxAmmo - gunAmmo;

            if (ammo > substr)
            {
                gunAmmo += substr;
                ammo -= substr;
            }
            else
            {
                gunAmmo += ammo;
                ammo = 0;
            }
        }
    }

    void Update()
    {
        float cRecoil = Mathf.Clamp(recoil, 0, maxRecoil);
        float cRechargeTime = Mathf.Clamp(rTime, 0, rechargeTime);

        if (gunHolder.childCount > 0)
            hasGun = true;
        else
            hasGun = false;

        if (hasGun)
        {
            playerAnim.SetBool("HasGun", true);
            ammoCount.gameObject.SetActive(true);
            ammoCount.text = gunAmmo.ToString() + "/" + ammo.ToString();
        }
        else
        {
            playerAnim.SetBool("HasGun", false);
            ammoCount.gameObject.SetActive(false);
        }
            

        if (isShooting && hasGun && gunAmmo > 0 && cRechargeTime <= 0)
        {            
            weight += 1 * Time.deltaTime * animSpeed;
            playerAnim.SetLayerWeight(1, weight);

            if (cRecoil <= 0 && weight >= 1)
            {
                Instantiate(bullet, cannon.position, cannon.rotation);
                gunAmmo -= 1;
                recoil = maxRecoil;
            }            
        }

        if (!isShooting)
        {
            weight -= 1 * Time.deltaTime * animSpeed;
            playerAnim.SetLayerWeight(1, weight);
        }

        if (recoil > 0)
            recoil -= 1f * Time.deltaTime;
        if (rTime > 0)
            rTime -= 1f * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (weight > 1) weight = 1;
        if (weight <= 0.1f) weight = 0;
    }
}
