using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float lifeSpan = 5f;
    float currentLife = 0f;
    [SerializeField] int damage = 10;

    private void Start()
    {
        currentLife = lifeSpan + Time.time;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (currentLife < Time.time)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            collision.GetComponent<PlayerHealth>().DealDamage(damage);
        }
        Destroy(gameObject);
    }
}
