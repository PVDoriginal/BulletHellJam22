using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    [SerializeField] GameObject tetherHead;
    [SerializeField] GameObject tetherTail;
    [SerializeField] GameObject tetherSprite;

    GameObject connectedObjectHead;
    GameObject connectedObjectTail;

    DistanceJoint2D tetherJoint;
    GameObject tetherSource;

    Vector3 terrainPoint;

    public GameObject GetHead()
    {
        return connectedObjectHead;
    }    

    public GameObject GetTail()
    {
        return connectedObjectTail;
    }

    public void AddTether(GameObject source, GameObject head, GameObject tail)
    {
        tetherSource = source;
        SetHead(head);
        SetTail(tail);
    }
    public void AddTether(GameObject source, Vector3 headPoint, GameObject tail)
    {
        tetherSource = source;
        SetHead(headPoint);
        SetTail(tail);
    }

    public void SetHead(GameObject head)
    {
        connectedObjectHead = head;
        tetherHead.transform.position = connectedObjectHead.transform.position;
    }

    public void SetHead(Vector3 headPoint)
    {
        connectedObjectHead = null;
        tetherHead.transform.position = headPoint;
        terrainPoint = headPoint;
    }

    public void SetTail(GameObject tail)
    {
        connectedObjectTail = tail;
    }

    public void RemoveTether()
    {
        if(tetherSource == null) { return; }
        

        Destroy(tetherSource.GetComponent<DistanceJoint2D>());

        Destroy(gameObject);
        
    }

    public void RemoveTether(DistanceJoint2D joint)
    {
        if(tetherSource == null) { return; }
        Destroy(joint);
        Destroy(gameObject);
    }

    private void Update()
    {
        tetherTail.transform.position = connectedObjectTail.transform.position;

        if(connectedObjectHead == null) 
        {
            tetherHead.transform.position = terrainPoint;
            return; 
        }
        tetherHead.transform.position = connectedObjectHead.transform.position;
    }

}
