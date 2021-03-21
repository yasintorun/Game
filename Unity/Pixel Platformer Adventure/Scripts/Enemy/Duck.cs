using UnityEngine;

public class Duck : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public Transform hitCheck;
    public LayerMask whatIsGround;

    [Space]
    public float waitTime;
    float timer = 0f, dir;
    

    Rigidbody2D rb;
    Animator anim;

    bool FacingRight = true;
    void Start()
    {
        timer = Random.Range(0, waitTime);
        dir = transform.localScale.x * -1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Collider2D c = Physics2D.OverlapCircle(hitCheck.position, checkRadius, whatIsGround);

        if (c)
        {
            if (rb.velocity.x > 0 && FacingRight)
                Flip();
            else if (rb.velocity.x < 0 && !FacingRight)
                Flip();
        }

        timer += Time.deltaTime;
        if(timer > waitTime)
        {
            anim.SetBool("isJumping", true);
            timer = 0f;
        }
        anim.SetFloat("jump", rb.velocity.y);

    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        dir *= -1;
    }

    public void jumping()
    {
        rb.velocity = new Vector2(1 * dir, 3) * speed;
        anim.SetBool("isJumping", false);

    }
    bool isDie = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isDie && collision.gameObject.CompareTag("Player"))
        {
            isDie = true;
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDie && collision.tag == "PlayerFoot")
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            isDie = true;
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            anim.SetBool("isDie", true);
        }
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.up * Random.Range(2, 5);
        Destroy(gameObject, 4);
    }

}
