using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBoxIntersectionVisualizer : MonoBehaviour
{
    public AABBPrimitive AABB;
    public RayPrimitive Ray;


    private void OnDrawGizmos()
    {
        if (Ray != null && AABB != null)
        {
            float t = intersect(Ray, AABB);
            if (t >= 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Ray.transform.position + Ray.transform.forward * t, 0.25f);
                Gizmos.color = Color.white;
            }
        }

    }

    private float intersect(RayPrimitive ray, AABBPrimitive aabb)
    {
        Vector3 rayDirection = ray.transform.forward;
        Vector3 inverseRayDirection = new Vector3(1.0f / rayDirection.x, 1.0f / rayDirection.y, 1.0f / rayDirection.z);
        int[] rayDirectionSigns = {
            inverseRayDirection.x < 0 ? 1:0,
            inverseRayDirection.y < 0 ? 1:0,
            inverseRayDirection.z < 0 ? 1:0
            };
        Vector3[] bounds =
        {
            aabb.transform.position - aabb.transform.localScale * 0.5f,
            aabb.transform.position + aabb.transform.localScale * 0.5f
        };

        float tmin, tmax, tymin, tymax, tzmin, tzmax;
        tmin = (bounds[rayDirectionSigns[0]].x - ray.transform.position.x) * inverseRayDirection.x;
        tmax = (bounds[1 - rayDirectionSigns[0]].x - ray.transform.position.x) * inverseRayDirection.x;
        tymin = (bounds[rayDirectionSigns[1]].y - ray.transform.position.y) * inverseRayDirection.y;
        tymax = (bounds[1 - rayDirectionSigns[1]].y - ray.transform.position.y) * inverseRayDirection.y;

        if ((tmin > tymax) || (tymin > tmax))
            return -1.0f;
        if (tymin > tmin)
            tmin = tymin;
        if (tymax < tmax)
            tmax = tymax;

        tzmin = (bounds[rayDirectionSigns[2]].z - ray.transform.position.z) * inverseRayDirection.z;
        tzmax = (bounds[1 - rayDirectionSigns[2]].z - ray.transform.position.z) * inverseRayDirection.z;

        if ((tmin > tzmax) || (tzmin > tmax))
            return -1.0f;
        if (tzmin > tmin)
            tmin = tzmin;
        if (tzmax < tmax)
            tmax = tzmax;

        return tmin;
    }

}
