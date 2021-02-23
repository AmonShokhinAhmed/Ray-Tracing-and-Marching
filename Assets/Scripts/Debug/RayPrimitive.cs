using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPrimitive : Primitive
{
    protected override void Draw()
    {
        float min = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);
        transform.localScale = new Vector3(min, min, min);
        Gizmos.DrawRay(transform.position, transform.forward * 100 );
    }
}
