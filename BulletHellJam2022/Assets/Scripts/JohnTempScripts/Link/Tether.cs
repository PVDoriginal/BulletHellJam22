using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    [SerializeField] GameObject tetherHead;
    [SerializeField] GameObject tetherTail;
    [SerializeField] GameObject tetherSprite;

    GameObject objectHead;
    GameObject objectTail;

    DistanceJoint2D tetherJoint;
    GameObject tetherSource;

    public GameObject GetTetherHead()
    {
        return tetherHead;
    }

    public GameObject GetTetherTail()
    {
        return tetherTail;
    }

    public GameObject GetTetherSprite()
    {
        return tetherSprite;
    }

    public void SetHead(GameObject head)
    {
        objectHead = head;
        tetherHead.transform.position = objectHead.transform.position;
    }

    public void SetTail(GameObject tail)
    {
        objectTail = tail;
    }

    public void AddTether(GameObject source)
    {
        tetherSource = source;
    }

    public void RemoveTether()
    {
        if(tetherSource == null) { return; }
        
        Destroy(tetherSource.GetComponent<DistanceJoint2D>());

        Destroy(gameObject);
        
    }

    private void Update()
    {
        tetherHead.transform.position = objectHead.transform.position;
        tetherTail.transform.position = objectTail.transform.position;
    }

}
