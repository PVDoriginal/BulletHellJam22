using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    bool chargeSet = false;
    int damage = 50;

    public void SetCharge()
    {
        chargeSet = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(chargeSet)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
            foreach(Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == 3)
                {
                    collider.gameObject.GetComponent<PlayerHealth>().DealDamage(damage);
                }
                else if (collider.tag == "rock" || collider.tag == "Enemy")
                {
                    Destroy(collider.gameObject);
                    Destroy(gameObject);
                }
            }

        }
    }


}
