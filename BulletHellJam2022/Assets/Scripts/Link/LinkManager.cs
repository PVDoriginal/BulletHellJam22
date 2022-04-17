using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{
    [SerializeField] private LineRenderer LR;
    [SerializeField] private GameObject LRPrefab;
    [SerializeField] private Transform LineParent;


    private GameObject G1;

    public void SetG1(GameObject G)
    {
        G1 = G;

        StartCoroutine(DrawLine());
    }

    public GameObject CurrObject;

    public void SetG2()
    {
        GameObject G = CurrObject;

        Clear();

        if (G == G1 || G1 == null) return;

        GameObject lr = Instantiate(LRPrefab, LineParent);
        StartCoroutine(lr.GetComponent<LineDrawer>().DrawLine(G1.transform, G.transform));
        
        G.GetComponent<LinkHandler>().CreateLink(G1.transform, 0);
        G1 = null; CurrObject = null;
    }

    public void Clear()
    {
        LR.enabled = false;
    }

    private IEnumerator DrawLine()
    {
        LR.enabled = true;

        while (G1 != null)
        {
            LR.SetPosition(0, G1.transform.position);
            LR.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            yield return new WaitForEndOfFrame();
        }
    }
}
