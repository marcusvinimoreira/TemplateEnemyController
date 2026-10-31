using UnityEngine;

public class ShowVisionCod : MonoBehaviour
{
    public MeshFilter meshF;
    private Mesh mesh;

    Vector3[] vertices;
    int[] triangle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh();
        meshF.mesh = mesh;

        vertices = new Vector3[]
        {
            new Vector3(-1,0,1),
            new Vector3(0,1,0),
            new Vector3(1,0,0),

        };

        mesh.vertices = vertices;
        triangle = new int[]
        {
            0,1,2,3
        };
        mesh.triangles = triangle;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
