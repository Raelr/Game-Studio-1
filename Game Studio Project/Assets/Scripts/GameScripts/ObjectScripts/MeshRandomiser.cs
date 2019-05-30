using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRandomiser : MonoBehaviour
{
    public List<Mesh> meshes;

    public MeshFilter filter;
    public MeshCollider collider;

    private void Start()
    {
        Mesh chosenMesh = meshes[Random.Range(0, meshes.Count)];

        if (filter)
            filter.mesh = chosenMesh;

        if (collider)
            collider.sharedMesh = chosenMesh;



    }
}
