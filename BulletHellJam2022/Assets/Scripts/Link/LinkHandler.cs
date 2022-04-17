using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkHandler : MonoBehaviour
{

    private LinkManager link;

    private void Start()
    {
        link = GameObject.Find("GameManager").GetComponent<LinkManager>();
    }

    private void OnMouseDown()
    {   
        link.SetG1(this.gameObject);
    }

    private void OnMouseEnter()
    {
        link.CurrObject = this.gameObject;
    }

    private void OnMouseUp()
    {
        link.SetG2();
    }

    public void CreateLink(Transform T, int type)
    {
        GetComponent<BasicEnemyScript>().SetEnemyConnection(T);
    }
}
