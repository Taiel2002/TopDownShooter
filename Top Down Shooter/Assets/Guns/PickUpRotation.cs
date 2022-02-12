using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRotation : MonoBehaviour
{
    public int speed;
    public GunType weapon;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Shooting.instance.gun = weapon;
            Shooting.instance.PickGun();
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(0,speed * Time.deltaTime,0);
    }
}
