using UnityEngine;

public class EnemyVisionController : MonoBehaviour
{
    [SerializeField]
    private float rayVision, rayAttack, speed;
    public LayerMask layer;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Vector3 posStart;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector3 target;
    [SerializeField]
    private float fov;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        posStart = transform.position;
        rb = GetComponent<Rigidbody2D>();
        target = posStart;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2.Angle(player.transform.position - transform.position, -transform.up)) <= fov *0.5f)
        {
            RayPlayer();

        }
    }

    //Metódo para calcular a distância do player e Enemy
    void RayPlayer()
    {
        //Verifica se o player está colidindo com a campo de visão do inimigo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, rayVision, layer);
        //Dispara uma linha em direção ao player 
        Vector3 temp = transform.TransformDirection(player.transform.position - transform.position);
        //Mostra o Gismos da linha
        Debug.DrawRay(transform.position, temp, Color.red);
        //Verifica se o hit está colidindo com o player
        //Se sim o Target(alvo) pega a posição do player
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;
            }
            else //quando perder a visão da tag ele volta para pos inicial
            {
                target = posStart;

            }
        }
        else//volta para área de incial quando player sai da área de visão
        {
            target = posStart;
        }

        float distanciaTime = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;

        //Verficar se é o player
        //se ele está área de ataque
        if (target != posStart && distanciaTime < rayAttack)
        {
            //animação
        }
        else
        {
            rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }

        if (target == posStart && distanciaTime < 0.02f)
        {
            transform.position = posStart;
        }
        Debug.DrawLine(transform.position, target, Color.green);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rayVision);
        Gizmos.DrawWireSphere(transform.position, rayAttack);
    }


}
