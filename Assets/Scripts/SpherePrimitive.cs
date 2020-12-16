using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePrimitive : Primitive
{
    private Vector3 _lastScale;
    protected override void DrawGizmos()
    {
        float min = Mathf.Max(Mathf.Max(transform.localScale.x,transform.localScale.y),transform.localScale.z);
        transform.localScale = new Vector3(min, min, min);
        Gizmos.DrawSphere(transform.position, transform.localScale.x *0.5f);
    }
}
