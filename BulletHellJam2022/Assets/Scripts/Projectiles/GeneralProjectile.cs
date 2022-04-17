using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    private bool beingDestroyed = false;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive || beingDestroyed) return;

        if (collision.gameObject.layer == 6) // line
            StartCoroutine(Dst());
    }

    public IEnumerator Move(Vector3 dir)
    {
        StartCoroutine(Activate());
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, (transform.position + dir), speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(0.05f);
        isActive = true;
    }

    private IEnumerator Dst()
    {
        beingDestroyed = true;
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
