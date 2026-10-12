using UnityEngine;

public class EnemyVisionController : MonoBehaviour
{
    [SerializeField]
    private float rayVision, rayAttack, speed;
    public LayerMask layer;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Vector2 posStart;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector3 target;

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
        RayPlayer();
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
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Player")
            {
                target = player.transform.position;
            }
        }

        float distanciaTempo = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rayVision);
        Gizmos.DrawWireSphere(transform.position, rayAttack);
    }


}
