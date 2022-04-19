using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody2D enemyRb;
    [SerializeField] GameObject player;

    private float initialDistance;
    private bool increasingDistance = false;
    Vector3 lookDirection;
    [SerializeField] float maxVelocity = 5f;

    void Start()
    {

        enemyRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        // Set enemy direction towards player goal and move there
        lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        enemyRb.velocity = Vector3.ClampMagnitude(enemyRb.velocity, maxVelocity);
        CheckIfTurningAround();
    }

    private void CheckIfTurningAround()
    {
        float currentDistance = Vector3.Distance(player.transform.position, transform.position);

        if(Mathf.Abs(Vector3.Dot(lookDirection, enemyRb.velocity.normalized)) < 0.05 && currentDistance > 5f)
        {
            enemyRb.velocity = Vector3.zero;
        }
    }
}
