using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer LR;
    [SerializeField] private EdgeCollider2D edge;
    [SerializeField] private Material WhiteMat, RedMat;

    private int t1, t2;
    GameObject G1, G2;

    public IEnumerator DrawLine(Transform T1, Transform T2, int type1, int type2)
    {
        t1 = type1; t2 = type2;
        G1 = T1.gameObject; G2 = T2.gameObject;

        Physics2D.IgnoreCollision(edge, T1.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(edge, T2.GetComponent<Collider2D>());

        while (true)
        {
            LR.widthMultiplier = Mathf.Lerp(0.3f, 1.5f, 1 - Mathf.Clamp(Vector2.Distance(T1.position, T2.position) / 3, 0, 1));

            Vector2 pos1, pos2;

            pos1 = T1.position + (T2.position - T1.position).normalized * 0.25f;
            pos2 = T2.position + (T1.position - T2.position).normalized * 0.25f;

            List<Vector2> ls = new List<Vector2>();

            ls.Add(pos1);
            ls.Add(pos2);

            edge.SetPoints(ls);

            LR.SetPosition(0, pos1);
            LR.SetPosition(1, pos2);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnMouseEnter()
    {
        LR.material = RedMat;
    }

    private void OnMouseExit()
    {
        LR.material = WhiteMat;
    }

    private void OnMouseDown()
    {
        EraseLine();
    }

    public void EraseLine()
    {
        G1.GetComponent<LinkHandler>().RemoveLink(G2.transform, t2);
        G2.GetComponent<LinkHandler>().RemoveLink(G1.transform, t1);

        StartCoroutine(DeleteTimer());

    }


    private IEnumerator DeleteTimer()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
