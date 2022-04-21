using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] GameObject player;
    [SerializeField] GameObject projectile;

    [SerializeField] float speed;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float turnSpeed;
    [SerializeField] float objectAvoidanceTurnSpeed = 10f;

    private float currentDistanceFromPlayer;
    Vector3 lookDirectionVector;
    private float lookDirection;
    bool isOnSameDiveRun = false;
    int turningLeftorRight = 1;
    [SerializeField] LayerMask layerMask = new LayerMask();

    float fireRate = 1.0f;
    float nextFire = 0;
    int collisionDamage = 30;



    void Update()
    {
        BasicThrusters();
        CheckIfTurningAround();
        CheckIfNearPlayer();
        CheckIfReadyToFire();
        CheckIfTerrainAhead();
    }

    private void CheckIfTerrainAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 3f, layerMask);
        if (hit)
        {
            Debug.Log("turning");
            enemyRb.AddForce(turningLeftorRight * transform.right * objectAvoidanceTurnSpeed);
        }
    }

    private void BasicThrusters()
    {
        lookDirectionVector = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirectionVector * speed);
        enemyRb.AddForce(transform.forward * 1f);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, enemyRb.velocity);
        enemyRb.velocity = Vector3.ClampMagnitude(enemyRb.velocity, maxSpeed);

        currentDistanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        lookDirection = Vector3.Dot(lookDirectionVector, enemyRb.velocity.normalized);
    }

    private void CheckIfReadyToFire()
    {
        if (lookDirection > 0.8f && currentDistanceFromPlayer < 6f && Time.time > nextFire)
        {
            Instantiate(projectile, transform.position, Quaternion.LookRotation(Vector3.forward, enemyRb.velocity));
            nextFire = Time.time + fireRate;
        }
    }

    private void CheckIfNearPlayer()
    {
        if(lookDirection > 0 && isOnSameDiveRun == false)
        {
            isOnSameDiveRun = true;
            turningLeftorRight = (Random.Range(0, 2) * 2) - 1;
        }
        else if(lookDirection < 0)
        {
            isOnSameDiveRun = false;
        }
        if (currentDistanceFromPlayer < 3f && enemyRb.velocity.magnitude > 2 && lookDirection > 0)
        {
            enemyRb.AddForce(turningLeftorRight * transform.right * (turnSpeed));
        }
    }

    private void CheckIfTurningAround()
    {
        if(Mathf.Abs(lookDirection) < 0.05f && currentDistanceFromPlayer > 4f)
        {
            enemyRb.velocity = Vector3.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.name == "TetherSprite")
        {
            Tether tether = collision.gameObject.GetComponent<TetherSprite>().GetTether();
            if(tether.GetHead() == gameObject || tether.GetTail() == gameObject) { return; }
            player.GetComponent<TetherSelection>().TetherDestroyed();
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.transform.tag == "terrain")
        {
            Destroy(gameObject);
        }
        else if(collision.transform.tag == "rock")
        {
            if(collision.gameObject.GetComponentInChildren<Tether>())
            {
                collision.gameObject.GetComponentInChildren<Tether>().RemoveTether();
            }
            player.GetComponent<TetherSelection>().TetherDestroyed();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
        }
        else if(collision.gameObject.layer == 3)
        {
            collision.gameObject.GetComponent<PlayerHealth>().DealDamage(collisionDamage);
        }
    }
}
