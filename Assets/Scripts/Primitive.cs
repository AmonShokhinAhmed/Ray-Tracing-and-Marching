using UnityEngine;

public abstract class Primitive : MonoBehaviour
{
    public Vector3 albedo;
    public Vector3 specular;

    private void OnDrawGizmos()
    {
        DrawGizmos();
    }

    protected abstract void DrawGizmos();
}
