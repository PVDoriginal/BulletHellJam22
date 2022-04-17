using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private GameObject tetheredFocus;

    [SerializeField] Rope rope;

    private Vector2 movement;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D targetObject = Physics2D.OverlapPoint(mousePos);

            if (targetObject)
            {
                if (!tetheredFocus)
                {
                    rope.GenerateRope(targetObject.GetComponent<HingeJoint2D>());
                    tetheredFocus = targetObject.gameObject;
                }
                else
                {
                    rope.GenerateRope(targetObject.gameObject, tetheredFocus);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
