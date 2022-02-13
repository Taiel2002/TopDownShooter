using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    [SerializeField] private Transform collisionVFX;
    public Vector3 origin;
    RaycastHit hit;

    private void Awake()
    {
        origin = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        StartCoroutine(DestroyBullet());
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 rotation = new Vector3(transform.rotation.x, transform.rotation.y - 180, transform.rotation.z);
        Instantiate(collisionVFX, transform.position, Quaternion.Euler(rotation)); ;
        Destroy(this.gameObject);
    }

    //private void FixedUpdate()
    //{
    //    if (Physics.Linecast(transform.position, (origin - transform.position), out hit))
    //    {
    //        if (hit.transform.tag != "Bullet")
    //        {
    //            Vector3 rotation = new Vector3(transform.rotation.x, transform.rotation.y - 180, transform.rotation.z);
    //            Instantiate(collisionVFX, transform.position, Quaternion.Euler(rotation)); ;
    //            Destroy(this.gameObject);
    //        }            
    //    }
    //}

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
