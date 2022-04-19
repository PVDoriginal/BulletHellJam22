using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetherSelection : MonoBehaviour
{
    [SerializeField] GameObject tether;
    GameObject tempTether;
    [SerializeField] List<GameObject> tetheredObjects = new List<GameObject>();

    Camera mainCamera;

    [SerializeField] LayerMask layerMask = new LayerMask();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            if(tether == null) { return; }
            tempTether.GetComponent<Tether>().RemoveTether();
        }
        if(!Mouse.current.leftButton.wasPressedThisFrame) { return; }
        Ray ray;

        Vector3 mousePosition = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
        
        if (hit)
        {
            tempTether = Instantiate(tether, hit.collider.gameObject.transform.position, Quaternion.identity, transform);
            tempTether.GetComponent<Tether>().SetHead(gameObject);
            tempTether.GetComponent<Tether>().SetTail(hit.collider.gameObject);
            tempTether.GetComponent<Tether>().AddTether(hit.collider.gameObject);

            DistanceJoint2D distantComponent = hit.collider.gameObject.AddComponent<DistanceJoint2D>();
            distantComponent.connectedBody = gameObject.GetComponent<Rigidbody2D>(); 
            distantComponent.distance = 2.0f;
            distantComponent.autoConfigureDistance = false;
            distantComponent.maxDistanceOnly = true;
        }
    }

    
}
