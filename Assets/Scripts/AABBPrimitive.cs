using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBPrimitive : Primitive
{
    protected override void Draw()
    {
        Vector3 min = transform.position - transform.localScale * 0.5f;
        Vector3 max = transform.position + transform.localScale * 0.5f;
        Vector3 a = min;
        Vector3 b = new Vector3(max.x, min.y, min.z);
        Vector3 c = new Vector3(max.x, min.y, max.z);
        Vector3 d = new Vector3(min.x, min.y, max.z); ;
        Vector3 e = new Vector3(min.x,max.y,min.z);
        Vector3 f = new Vector3(max.x, max.y, min.z);
        Vector3 g = new Vector3(max.x, max.y, max.z);
        Vector3 h = new Vector3(min.x, max.y, max.z); ;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);

        Gizmos.DrawLine(a, e);
        Gizmos.DrawLine(b, f);
        Gizmos.DrawLine(c, g);
        Gizmos.DrawLine(d, h);

        Gizmos.DrawLine(e, f);
        Gizmos.DrawLine(f, g);
        Gizmos.DrawLine(g, h);
        Gizmos.DrawLine(h, e);
    }
}
