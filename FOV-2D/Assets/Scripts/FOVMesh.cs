using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMesh : MonoBehaviour
{
    //FOV script
    FOV fov;

    //Desenho
    Mesh mesh;

    //acerto
    RaycastHit2D hit;

    //Resolucao
    [SerializeField] float meshRes = 2;

    //vertices
    [HideInInspector] public Vector3[] vertices;

    //triangulacao de pontos
    [HideInInspector] public int[] triangles;

    //contador de passos
    [HideInInspector] public int stepCount;


    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        fov = GetComponentInParent<FOV>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MakeMesh();
    }

    private void MakeMesh()
    {
        stepCount = Mathf.RoundToInt(fov.viewAngle * meshRes);
        float stepAngle = fov.viewAngle / stepCount;

        List<Vector3> viewVertex = new List<Vector3>();

        hit = new RaycastHit2D();

        for (int i = 0; i < stepCount; i++)
        {
            float angle = fov.transform.eulerAngles.y - fov.viewAngle / 2 + stepAngle * i;
            Vector3 dir = fov.DirFromAngle(angle, false);

            //armazena o obstaculo
            hit = Physics2D.Raycast(fov.transform.position, dir, fov.viewRadius, fov.obstacleMask);

            //se o collider do obstaculo for nulo
            if (hit.collider == null)
            {
                viewVertex.Add(transform.position + dir.normalized * fov.viewRadius);
            }
            else
            {
                viewVertex.Add(transform.position + dir.normalized * hit.distance);
            }
        }

        int vertexCount = viewVertex.Count + 1;

        vertices = new Vector3[vertexCount];
        triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewVertex[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3 + 2] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
