using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletSpawn : MonoBehaviour
{
    public GameObject bullet;

    private float nextActionTime = 0.0f;
    public float period;

    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
