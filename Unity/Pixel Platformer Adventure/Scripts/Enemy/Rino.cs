using UnityEngine.Internal;
using UnityEngine;

public class Rino : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed, geriTepmeHizi;
    Animator anim;
    public bool isUnDead = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        timer = waitTime;
        //rb.velocity = Vector2.left * 20 * Time.deltaTime;
    }
    bool run = false, FacingRight = true;
    Vector2 dir = Vector2.left;
    Vector2 force;

    public float waitTime;
    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir);
            foreach (var h in hit)
            {
                if (h.collider.gameObject.layer == 8) break; //ground

                if (h.collider.tag == "Player" && !run)
                {
                    if (dir == Vector2.right && FacingRight)
                        Flip();
                    else if (dir == Vector2.left && !FacingRight)
                        Flip();
                    force = dir;
                    run = true;
                }
            }
            if (run)
                rb.AddForce(force * speed * Time.deltaTime);
            dir *= -1;
        }
        anim.SetBool("run", run);
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isDie)
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            enabled = false;
            anim.SetBool("run", false);
            isDie = true;
            return;
        }
        if (run)
        {
            timer = 0f;
            anim.SetBool("hitwall", true);
            rb.velocity =  (force.x > 0 ? new Vector2(-1.5f, 2) : new Vector2(1.5f, 2)) * geriTepmeHizi;
        }
        run = false;
    }

    bool isDie = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFoot") && !isDie && !isUnDead)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            collision.transform.parent.gameObject.GetComponent<PlayerController>().JumpPlayer();
            Die();
            anim.SetBool("isDie", true);
            enabled = false;
            isDie = true;
        
        }
    }
    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        rb.velocity = Vector2.up * Random.Range(2, 5);
    }

    public void ResetHitWall()
    {
        anim.SetBool("hitwall", false);
    }

}
