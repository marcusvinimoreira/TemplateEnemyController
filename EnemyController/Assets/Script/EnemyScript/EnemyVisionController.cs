using UnityEngine;
using System.Collections.Generic;

public class EnemyVisionController : MonoBehaviour
{
    public float rayVision, rayAttack, speed;
    public LayerMask layer;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Vector3 posStart;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector3 target;
    public float fov;

    private bool canRotation = false;

    //
    public Animator anim;
    private Vector3 dir;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    public float meshRes;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;

        player = GameObject.FindGameObjectWithTag("Player");
        posStart = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = posStart;
    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Vector2.Angle(player.transform.position - transform.position, -transform.up)) <= fov * 0.5f)
        {
            RayPlayer();

        }
        else
        {
            BackPost();
        }
    }

    private void LateUpdate()
    {
        DrawFielofView();

    }
    //Metódo para calcular a distância do player e Enemy
    void RayPlayer()
    {
        //Verifica se o player está colidindo com a campo de visão do inimigo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, rayVision, layer);
        //Dispara uma linha em direção ao player 
        Vector3 temp = player.transform.position - transform.position;
        //Mostra o Gismos da linha
        Debug.DrawRay(transform.position, temp, Color.red);
        //Verifica se o hit está colidindo com o player
        //Se sim o Target(alvo) pega a posição do player
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;
                //  canRotation = true;
            }
            else //quando perder a visão da tag ele volta para pos inicial
            {
                target = posStart;
                canRotation = false;
            }
        }
        else//volta para área de incial quando player sai da área de visão
        {
            target = posStart;
            canRotation = false;

        }

        float distanciaTime = Vector3.Distance(target, transform.position);
        dir = (target - transform.position).normalized;

        //Verficar se é o player
        //se ele está área de ataque
        if (target != posStart && distanciaTime < rayAttack)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
            anim.SetBool("isAttack", false);

        }

        if (target == posStart && distanciaTime < 0.02f)
        {
            transform.position = posStart;
            anim.SetFloat("x", 0);
            anim.SetFloat("x", 0);
        }
        Debug.DrawLine(transform.position, target, Color.green);

        if (canRotation)
        {
            transform.up = (player.transform.position - transform.position) * -1;
            if (distanciaTime > rayAttack)
            {
                rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
            }
        }
        else
        {
            transform.up = (posStart - transform.position) * -1;
        }
    }

    //Metódo para visualizar os angulos de ataque do inimigo
    public Vector3 DirectionAngle(float angDeg)
    {
        angDeg -= transform.eulerAngles.z;
        return new Vector3(Mathf.Sin(angDeg * Mathf.Deg2Rad), Mathf.Cos(angDeg * Mathf.Deg2Rad), 0);
    }

    void BackPost()
    {
        dir = (target - transform.position).normalized;
        float distanceTemp = Vector3.Distance(target, transform.position);
        if (target == posStart && distanceTemp < 0.02f)
        {
            transform.position = posStart;
            anim.SetFloat("x", dir.x);
            anim.SetFloat("x", dir.y);
            transform.up = Vector3.zero;
        }
        if (transform.position != posStart)
        {
            rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rayVision);
        Gizmos.DrawWireSphere(transform.position, rayAttack);
    }

    void DrawFielofView()
    {
        int stepCount = Mathf.RoundToInt(fov * meshRes); //Mathf.RoundToInt arredonda para inteiro
        float stepAngle = fov / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - fov / 2 + stepAngle * i;
            ViewStruct auxView = View(angle);

            Debug.DrawLine(transform.position, transform.position - DirectionAngle(angle) * rayVision, Color.green);
            viewPoints.Add(auxView.pos);

        }

        int vertexCount = viewPoints.Count + 1;

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;

            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    public struct ViewStruct
    {
        public Vector3 pos;
        public ViewStruct(Vector3 _pos)
        {
            pos = _pos;
        }
    }

    ViewStruct View(float gloabalAngle)
    {
        Vector3 dir = -DirectionAngle(gloabalAngle);

        return new ViewStruct(transform.position + dir * rayVision);
    }
}
