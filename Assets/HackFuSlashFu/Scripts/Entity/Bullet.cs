using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Bullet : MonoBehaviour
{
    public GameObject PrefabOnDestroy;
    public float BulletMaxDuration = 3f;
    public OnCollider2DEvents ColliderEvents;

    void Start()
    {
        if (ColliderEvents)
            ColliderEvents.OnTriggerEnterTransformEvent.AddListener(OnTargetEnter);
        Invoke("DestroyObject", BulletMaxDuration);
    }

    void OnTargetEnter(Transform target)
    {
        Entity entity = GetComponent<Entity>();
        if (entity)
        {
            Debug.Log("Bullet Damage on " + entity);
            entity.ApplyDamage(1);
            DestroyObject();
        }

    }
    
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void Destroy()
    {
        if (PrefabOnDestroy)
        {
            GameObject go = Instantiate(PrefabOnDestroy);
            go.transform.position = transform.position;
        }
    }


}
