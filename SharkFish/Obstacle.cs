using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float radius;

    public float Radius => radius;

    public MeshRenderer mesh;

    private void Start()
    {
        radius = mesh.bounds.extents.x; // Sets radius
    }
}
