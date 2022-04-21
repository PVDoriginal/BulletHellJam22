using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetherSelection : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject tether;
    GameObject tempTether;
    [SerializeField] List<GameObject> tetheredObjects = new List<GameObject>();
    [SerializeField] float tetherRange = 2.0f;
    [SerializeField] float tetherSelectionRange = 2.0f;

    Camera mainCamera;

    [SerializeField] LayerMask layerMask = new LayerMask();

    private void Start()
    {
        mainCamera = Camera.main;
    }
 
    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame) 
        {
            SetTether(); 
        }

        if (Input.GetKeyDown("space"))
        {
            BreakTether();
        }
    }

    public void TetherDestroyed()
    {
        tetheredObjects.Clear();
        tempTether = null;
    }

    private void SetTether()
    {
        Vector3 mousePosition = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
        if(Vector3.Distance(hit.point, transform.position) > tetherSelectionRange) { return; }

        if (!hit) { return; }
        if (hit.collider.gameObject.layer != 7 && tetheredObjects.Count == 0)
        {
            tempTether = Instantiate(tether, 
                hit.collider.gameObject.transform.position, 
                Quaternion.identity, 
                hit.collider.gameObject.transform);

            tempTether.GetComponent<Tether>().AddTether(hit.collider.gameObject, gameObject, hit.collider.gameObject);
            tetheredObjects.Add(hit.collider.gameObject);

            DistanceJoint2D distantComponent = hit.collider.gameObject.AddComponent<DistanceJoint2D>();
            distantComponent.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            distantComponent.distance = tetherRange;
            distantComponent.autoConfigureDistance = false;
            distantComponent.maxDistanceOnly = true;
        }
        else
        {
            DistanceJoint2D[] connections = tetheredObjects[^1].GetComponents<DistanceJoint2D>();
            foreach (DistanceJoint2D connection in connections)
            {
                Debug.Log(connection.connectedBody.name);
                if(connection.connectedBody.name != "JohnPlayer") { continue; }
                if(hit.collider.gameObject.layer != 7)
                {
                    connection.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                    tempTether.GetComponent<Tether>().SetHead(hit.collider.gameObject);
                }
                else
                {
                    connection.connectedBody = null;
                    connection.connectedAnchor = hit.point;
                    tempTether.GetComponent<Tether>().SetHead(hit.point);
                }
                tetheredObjects.Clear();
                tempTether = null;
            }
        }
        if(hit.collider.tag == "Explosive")
        {
            hit.collider.GetComponent<Explosive>().SetCharge();
        }

        /*
        else if (hit.collider.gameObject.layer != 7)
        {
            Debug.Log(tetheredObjects[^1]);
            Debug.Log(tempTether);
            tetheredObjects[^1].GetComponent<DistanceJoint2D>().connectedBody =
                hit.collider.gameObject.GetComponent<Rigidbody2D>();
            tempTether.GetComponent<Tether>().SetHead(hit.collider.gameObject);
            tetheredObjects.Clear();
            tempTether = null;
        }
        else if (hit.collider.gameObject.layer == 7)
        {
            tetheredObjects[0].GetComponent<DistanceJoint2D>().connectedBody = null;
            tetheredObjects[0].GetComponent<DistanceJoint2D>().connectedAnchor = hit.point;
            tempTether.GetComponent<Tether>().SetHead(hit.point);
            tetheredObjects.Clear();
            tempTether = null;
        }*/
    }

    private void BreakTether()
    {
        if (tetheredObjects.Count == 0) { return; }
        

        DistanceJoint2D[] connections = tetheredObjects[^1].GetComponents<DistanceJoint2D>();

        foreach (DistanceJoint2D connection in connections)
        {
            if(connection.connectedBody == null) { continue; }
            if (connection.connectedBody.name != "JohnPlayer") { continue; }
            tempTether.GetComponent<Tether>().RemoveTether(connection);
        }
        tetheredObjects.Clear();
        tempTether = null;
    }

    
}
