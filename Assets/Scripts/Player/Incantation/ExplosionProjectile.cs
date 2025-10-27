using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionProjectile : MonoBehaviour
{
    [HideInInspector] public float explosionRadius = 3f;
    [HideInInspector] public float damage = 20f;

    public float lifetime = 3f;
    public LayerMask enemyLayer;   

    void Start() => Destroy(gameObject, lifetime);

    void OnCollisionEnter(Collision col)
    {

        Collider[] hitTargets = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (var hit in hitTargets)
        {
            if (hit.TryGetComponent(out IDamageable dmg))
                dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}