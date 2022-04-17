using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{
    [SerializeField] private LineRenderer LR;
    [SerializeField] private GameObject LRPrefab;
    [SerializeField] private Transform LineParent;

    [SerializeField] private Material WhiteMat, RedMat;


    private GameObject G1;

    private bool isValid = true;

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

        if (G == G1 || G1 == null || !isValid) return;

        GameObject lr = Instantiate(LRPrefab, LineParent);
        StartCoroutine(lr.GetComponent<LineDrawer>().DrawLine(G1.transform, G.transform));

        int type1 = 0, type2 = 0;

        if (G1.tag == "basicEnemy")
            type1 = 0;
        else if (G1.tag == "envObject")
            type1 = 1;

        if (G.tag == "basicEnemy")
            type2 = 0;
        else if (G.tag == "envObject")
            type2 = 1;


        G.GetComponent<LinkHandler>().CreateLink(G1.transform, type1);
        G1.GetComponent<LinkHandler>().CreateLink(G.transform, type2);
        G1 = null; CurrObject = null;
    }

    public void Clear()
    {
        LR.enabled = false;
    }

    private bool CheckIfValid()
    {
        float dis = Vector2.Distance(G1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (dis >= 3)
            return false;

        RaycastHit2D[] rh = Physics2D.LinecastAll(G1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        foreach (RaycastHit2D RH in rh)
            if (RH.transform.tag == "Line")
                return false;
        return true;
    }

    private IEnumerator DrawLine()
    {
        LR.enabled = true;

        while (G1 != null)
        {
            isValid = CheckIfValid();

            LR.widthMultiplier = Mathf.Lerp(0.3f, 1.5f, 1 - Mathf.Clamp(Vector2.Distance(G1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) / 3, 0, 1));

            if (isValid) LR.material = WhiteMat; else LR.material = RedMat;

            LR.SetPosition(0, G1.transform.position);
            LR.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            yield return new WaitForEndOfFrame();
        }
    }
}
