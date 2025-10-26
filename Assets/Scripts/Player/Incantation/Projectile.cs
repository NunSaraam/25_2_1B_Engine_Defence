using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 10f;
    private float damage;
    private float lifeTime = 5f;

    public void SetDamage(float value)
    {
        damage = value;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed *  Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy target = other.GetComponent<Enemy>();
        if (target != null && other.CompareTag("Enemy"))
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
