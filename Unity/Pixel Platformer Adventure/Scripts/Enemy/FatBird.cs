/*
*****BUG*****
BU SCRIPT DOĞRU ÇALIŞMIYOR. DÜŞMAN YERE DÜŞTÜGÜNDE VE OYUNCUYA DEGDİGİ ANDA ÖLMESİ GEREKİRKEN ÖLMÜYOR.
*/
using System.Data.Common;
using UnityEngine;

public class FatBird : MonoBehaviour
{
    public float speed, delay, waitTime;
    public Transform distance;

    Rigidbody2D rb;
    Animator anim;
    float dis = 0, timer = 0;
    Vector2 basePos;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dis = transform.position.y - distance.position.y;
        basePos = transform.position;
    }
    bool isFall = false;
    void FixedUpdate()
    {
        if (isDie) return;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.down, dis);
        foreach (var h in hit)
        {
            if (h.collider.CompareTag("Player") && !isFall)
            {
                isFall = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = Vector2.down *Random.Range(2, 5);
            }
        }
        if(isFall && Vector2.Distance(transform.position, distance.position) < delay)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                timer = 0f;
                isFall = false;
            }
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if(!isFall && Vector2.Distance(transform.position, basePos) > delay)
        {
            Debug.Log("asd");
            transform.position = Vector2.MoveTowards(transform.position, basePos, speed * Time.fixedDeltaTime);
        }

        anim.SetFloat("speed", Mathf.Abs(rb.velocity.y));
        anim.SetBool("ground", isFall);
    }
    bool isDie = false, kill = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFoot") && !isDie && !kill)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            isDie = true;
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            anim.SetBool("hit", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isDie)
        {
            kill = true;
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            collision.gameObject.GetComponent<PlayerController>().DeathPlayer();
            
        }

    }


    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = Vector2.up * Random.Range(1, 4);
    }

}
