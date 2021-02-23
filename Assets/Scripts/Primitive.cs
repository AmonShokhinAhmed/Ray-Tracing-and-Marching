using UnityEngine;

public abstract class Primitive : MonoBehaviour
{
    public Vector3 albedo;
    public Vector3 specular;
    public bool DebugDraw = true;
    private void OnDrawGizmos()
    {
        if (DebugDraw)
        {
            Draw();
        }
    }

    protected abstract void Draw();
}
