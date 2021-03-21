using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float speed = 3;
    public Transform left, right;
    Animator anim;
    Rigidbody2D rb;
    Transform player;
    public Transform check;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    bool FacingRight = true;
    void FixedUpdate()
    {
        if (isDie) return;
        var dir = (transform.position.x - player.position.x < 0) ? Vector2.right : Vector2.left;

        if (dir == Vector2.right && FacingRight) Flip();
        else if (dir == Vector2.left && !FacingRight) Flip();
        var distance = transform.position.x - ((dir == Vector2.left) ? left.position.x : right.position.x); 
        RaycastHit2D hit = Physics2D.Raycast(check.position, dir, Mathf.Abs(distance));
        if (hit.collider != null && hit.collider.gameObject == player.gameObject)
        {
            rb.velocity = dir * speed * Time.fixedDeltaTime;
        }
        else
            rb.velocity = Vector2.zero;
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }
    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        FacingRight = !FacingRight;
    }

    bool isDie = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isDie)
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            rb.velocity = Vector2.zero;
            anim.SetFloat("speed", 0);
            isDie = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerFoot") && !isDie)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            isDie = true;
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            anim.SetBool("hit", true);
        }
    }

    public void Die()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        rb.velocity = Vector2.up * Random.Range(2, 5);
    }

}
