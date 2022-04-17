using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer LR;
    [SerializeField] private EdgeCollider2D edge;

    public IEnumerator DrawLine(Transform T1, Transform T2)
    {
        Physics2D.IgnoreCollision(edge, T1.GetComponent<CircleCollider2D>());
        Physics2D.IgnoreCollision(edge, T2.GetComponent<CircleCollider2D>());

        while (true)
        {
            LR.widthMultiplier = Mathf.Lerp(0.3f, 1.5f, 1 - Mathf.Clamp(Vector2.Distance(T1.position, T2.position) / 3, 0, 1));

            List<Vector2> ls = new List<Vector2>();

            ls.Add(T1.position);
            ls.Add(T2.position);

            edge.SetPoints(ls);

            LR.SetPosition(0, T1.position);
            LR.SetPosition(1, T2.position);

            yield return new WaitForEndOfFrame();
        }
    }
}
