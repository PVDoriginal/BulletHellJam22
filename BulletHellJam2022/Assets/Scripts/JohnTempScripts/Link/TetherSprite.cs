using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSprite : MonoBehaviour
{
    [SerializeField] GameObject tetherHead;
    [SerializeField] GameObject tetherTail;

    private void Update()
    {
        Stretch(tetherHead.transform.position, tetherTail.transform.position);
    }

    private void Stretch(Vector3 tetherHead, Vector3 tetherTail)
    {
        
        Vector3 direction = Vector3.Normalize(tetherHead - tetherTail);
        transform.right = direction;
        
        Vector3 scale = new Vector3(1, 1, 1);
        float distance = Vector3.Distance(tetherHead, tetherTail);
        scale.x = distance;
        scale.y = (float)Mathf.Clamp(0.3f / distance, 0.002f, 0.3f);
        transform.localScale = scale;

        Vector3 center = (tetherHead + tetherTail) / 2f;
        transform.position = center;
        transform.localPosition += new Vector3(0, transform.localScale.y/2, 0);
    }

}
