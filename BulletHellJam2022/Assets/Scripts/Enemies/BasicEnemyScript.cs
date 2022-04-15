using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minDistanceToPlayer = 2f;

    [SerializeField] private Rigidbody2D Rb;
    private Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.position) > minDistanceToPlayer)
            Rb.MovePosition(Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime));
        else if(Vector2.Distance(transform.position, player.position) < minDistanceToPlayer - 0.3f)
            Rb.MovePosition(Vector2.MoveTowards(transform.position, transform.position + (transform.position - player.position) * 100, speed * Time.fixedDeltaTime));
        
    }

}
