using UnityEngine;

public class Chameleon : MonoBehaviour
{
    public float speed;
    public float distance;
    private bool FacingRight = false;
    public Transform left, right, characters;
    public Transform check;
    Animator anim;
    Rigidbody2D rb;
    private Transform player;

    public BoxCollider2D attackCollider;

    void Start()
    {
        for (int i = 0; i < characters.childCount; i++)
            if (characters.GetChild(i).gameObject.activeSelf)
                player = characters.GetChild(i);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Oyuncu Düşmana yakın mı? y eksenin de -0.7<y<2 aralıgında çalışır
        if (transformCheck("y"))
        {
            if (transform.position.x > player.position.x && FacingRight)
                Flip();
            else if (transform.position.x < player.position.x && !FacingRight)
                Flip();
            if (Mathf.Abs(player.position.x - transform.position.x) < distance)
            {
                anim.SetBool("isAttack", true);
                anim.SetBool("isRunning", false);
            }
            //x ekseninde left<player<right şeklinde.
            else if (transformCheck("x"))
            {
                attackCollider.enabled = false;
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunning", true);
                Vector3 target = new Vector3(player.position.x, transform.position.y, 0);
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            }
        }
        else {
            attackCollider.enabled = false;
            anim.SetBool("isAttack", false);
            anim.SetBool("isRunning", false);
        }

    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        FacingRight = !FacingRight;
    }

    bool transformCheck(string axis)
    {
        if (axis == "y")
            return Mathf.Abs(player.position.y - transform.position.y) < .7f || (player.position.y > transform.position.y && player.position.y - transform.position.y < 2);
        else
            return (left.position.x < player.position.x && player.position.x < right.position.x);
    }
    bool isDie = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isDie)
        {
            isDie = true;
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            enabled = false;
            anim.SetBool("isAttack", false);
            anim.SetBool("isRunning", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackCollider.IsTouching(collision))
            return;
        if(collision.gameObject.CompareTag("PlayerFoot") && !isDie)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            isDie = true;
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            anim.SetBool("hit", true);
            enabled = false;
        }
    }
    public void Die()
    {
        attackCollider.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.up * Random.Range(2, 5);
    }
    public void AttackCollider(int e)
    {
        if (e == 1)
            attackCollider.enabled = true;
        else
            attackCollider.enabled = false;
    }


}
