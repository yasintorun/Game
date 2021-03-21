using UnityEngine;

public class Plant : MonoBehaviour
{

    public float AttackWaitTime;
    public Transform checkPos;
    public GameObject bullet;
    Vector2 v2;
    float timer = 4f;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        v2 = transform.localScale.x == 1 ? Vector2.left : Vector2.right;
    }

    void Update()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(checkPos.position, v2);
        foreach (RaycastHit2D h in hit)
        {
            if (h && h.collider.tag == "Player")
            {
                timer += Time.deltaTime;
                if (timer > AttackWaitTime)
                {
                    anim.SetTrigger("attack");
                    timer = 0f;
                }
            }
        }
    }

    public void Attack()
    {
        GameObject b = Instantiate(bullet, checkPos.position, Quaternion.identity);
        b.GetComponent<Bullet>().Shot(v2);
    }

    bool isDie  = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isDie)
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            isDie = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerFoot") && !isDie)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            anim.SetBool("isDie", true);
            isDie = true;
        }
    }

    void EnemyDead()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * Random.Range(4, 7);
        enabled = false;
    }


}
