using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 1;
    public int RepelForceMultiplier = 4;

    void OnCollisionEnter2D(Collision2D other)
    {
        Entity entity = other.gameObject.GetComponentInChildren<Entity>();
        PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
        if (pc && pc.IsAttacking)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(pc.transform.right * rb.velocity.magnitude * RepelForceMultiplier, ForceMode2D.Impulse);
            return;
        }

        if (entity)
            entity.ApplyDamage(1);

        Destroy(gameObject);
    }
}
