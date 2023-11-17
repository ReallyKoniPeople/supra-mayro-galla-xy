using UnityEngine;

/// <summary>
/// This script can be used to reduce the amount of polygons for 3D-Models.
/// </summary>
public class MeshSimplifier : MonoBehaviour
{
    /// <summary>
    /// A factor that the total amount of polygons gets multiplied.
    /// Factor has to be lower for objects like spheres and higher for objects like cubes.
    /// </summary>
    public float quality = 0.01f;

    void Start()
    {
        var originalMesh = GetComponent<MeshFilter>().sharedMesh;
        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Initialize(originalMesh);
        meshSimplifier.SimplifyMesh(quality);
        var destMesh = meshSimplifier.ToMesh();
        GetComponent<MeshFilter>().sharedMesh = destMesh;
    }
}
