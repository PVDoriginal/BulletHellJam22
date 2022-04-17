using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float minDistanceToPlayer = 2f, maxDistanceToPlayer = 3f; // enemy needs to stay within 2 and 3 units of the player

    [SerializeField] private Rigidbody2D Rb;
    private Transform player;

    [SerializeField] private List<Transform> ConnectedEnemies;

    [SerializeField] private GameObject ProjectilePrefab;

    private bool beingPulled = false;

    [SerializeField] private float cooldown = 1.5f;

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        minDistanceToPlayer = Random.Range(0.5f, 1f);
        maxDistanceToPlayer = Random.Range(3f, 5.75f);

        StartCoroutine(Pull());
        StartCoroutine("ChangeOffset");
        StartCoroutine(Shoot());
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
        if (state == 2) // move closer
            Move(player.position, speed);
        else if (state == 0) // move away
            Move(transform.position + (transform.position - player.position) * 100, speed);
        else // move around
            Move(transform.position + offset, speed / 6);
    }

    private void Move(Vector3 target, float sp)
    {
        if (beingPulled) return;
        Rb.velocity = (target - transform.position).normalized * sp * Time.fixedDeltaTime;
    }
    
    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 3f));

        while(true)
        {
            if (state == 1)
            {
                GameObject bullet = Instantiate(ProjectilePrefab, transform);

                if(bullet != null)
                    StartCoroutine(bullet.GetComponent<GeneralProjectile>().Move((player.position - transform.position)));
            }
            yield return new WaitForSeconds(cooldown + Random.Range(0.1f, 0.35f));
        }
    }


    private IEnumerator Pull()
    {
        while(true)
        {
            foreach (Transform T in ConnectedEnemies)
            {
                float dist = Vector2.Distance(T.position, transform.position);

                if (dist > 3f)
                    StartCoroutine(T.gameObject.GetComponent<BasicEnemyScript>().PullTowards(transform));
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator PullTowards(Transform target)
    {
        beingPulled = true;

        while (Vector2.Distance(target.position, transform.position) > 3f) 
        {
            Rb.velocity = (target.position - transform.position).normalized * 60 * Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        beingPulled = false;
    }

    public void SetEnemyConnection(Transform T)
    {
        ConnectedEnemies.Add(T);
    }
}
