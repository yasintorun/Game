using UnityEngine;

public class Snail : MonoBehaviour
{
    AIMovement ai;
    Animator anim;
    Rigidbody2D rb;
    public GameObject SnailWithoutShell;
    void Start()
    {
        ai = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    bool shellCollision = false;
    public float shellSpeed;
    void Update()
    {
        //with shell
        if (!isDie)
        {
            ai.Move(true);
            anim.SetBool("walk", ai.GetIsMove());
        } else if(shellCollision)
        {
            if(rb.bodyType == RigidbodyType2D.Dynamic)
                rb.velocity = v2 * shellSpeed * Time.deltaTime;
        }
    }
    Vector2 v2;
    bool isDie = false, isShellDie = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!shellCollision && isDie)
            {
                ai.enabled = false;
                v2 = Random.Range(1, 3) == 1 ? Vector2.left : Vector2.right;
                shellCollision = true;
            }

            if (isDie) return;
            col.gameObject.GetComponent<PlayerController>().PlayerDead();
            isDie = true;
        } else
        {
            shellSpeed *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlayerFoot"))
        {
            if (!isDie)
            {
                anim.SetBool("hit", true);
                isDie = true;
                col.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            }
            else if (!shellCollision)
            {
                shellCollision = true;
                col.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            }
            else if (!isShellDie)
            {
                //anim.SetBool("shellDie", true);
            }
        }
    }

    public void Die()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.up * Random.Range(1, 4);
        Instantiate(SnailWithoutShell, transform.position, Quaternion.identity);
        anim.SetTrigger("noshell");
    }



}
