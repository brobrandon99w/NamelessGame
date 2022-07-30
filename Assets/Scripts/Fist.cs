using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public GameObject hitEffect;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
    //    Destroy(effect, 0.1f);
    //    Destroy(gameObject);
    //}

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Crate"))
                hitInfo.collider.GetComponent<Crate>().TakeDamage(damage);
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 0.1f);
        Destroy(gameObject);
    }
}
