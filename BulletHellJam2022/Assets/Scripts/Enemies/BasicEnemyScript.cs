using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float minDistanceToPlayer = 2f, maxDistanceToPlayer = 3f; // enemy needs to stay within 2 and 3 units of the player

    [SerializeField] private Rigidbody2D Rb;
    private GameObject playerGameObject;
    private Transform player;

    private void Start()
    {
        playerGameObject = GameObject.Find("Player");
        player = playerGameObject.transform;

        minDistanceToPlayer = Random.Range(1.5f, 2.3f);
        maxDistanceToPlayer = Random.Range(2.4f, 3.5f);

        StartCoroutine("ChangeOffset");
    }

    private Vector3 offset = Vector3.zero;
    private int state = 1;

    private void Update()
    {
        float disToPlayer = Vector2.Distance(transform.position, player.position);

        if (disToPlayer > maxDistanceToPlayer)
            state = 2;
        else if (disToPlayer < minDistanceToPlayer)
            state = 0;
        else
            state = 1;
        
    }

    private IEnumerator ChangeOffset()
    {
        while(true)
        {
            offset.x = Random.Range(-3, 3);
            offset.y = Random.Range(-3, 3);

            yield return new WaitForSeconds(1);
        }
    }

    private void FixedUpdate()
    {
        if (state == 2)
            Rb.MovePosition(Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime));
        else if (state == 0)
            Rb.MovePosition(Vector2.MoveTowards(transform.position, transform.position + (transform.position - player.position) * 100, speed * Time.fixedDeltaTime));
        else
            Rb.MovePosition(Vector2.MoveTowards(transform.position, transform.position + offset, speed / 6 * Time.fixedDeltaTime));
    }

}
