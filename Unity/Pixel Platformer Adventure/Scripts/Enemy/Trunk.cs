using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Trunk : MonoBehaviour
{

    public float AttackWaitTime;
    public Transform checkPos;
    public GameObject bullet;
    Vector2 v2;
    float timer = 4f;
    Animator anim;
    AIMovement ai;
    void Start()
    {
        anim = GetComponent<Animator>();
        ai = GetComponent<AIMovement>();
        v2 = transform.localScale.x == 1 ? Vector2.left : Vector2.right;
    }
    bool attack = false, attackDo = false;
    void Update()
    {
        v2 = transform.localScale.x == 1 ? Vector2.left : Vector2.right;
        RaycastHit2D[] hit = Physics2D.RaycastAll(checkPos.position, v2);
        attack = false;
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider.gameObject.layer == 8) break; //ground
            if (h && h.collider.tag == "Player")
            {
                timer = 0f;
                attack = true;
                break;
            }
        }
        

        if (attack == false)
        {
            anim.SetBool("attack", false);
            timer += Time.deltaTime;
            if (timer < AttackWaitTime)
            {
                anim.SetBool("run", false);
                return;
            }
            ai.Move(true);
            anim.SetBool("run", ai.GetIsMove());
        } else
        {
            anim.SetBool("attack", true);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ai.Ai_Collision(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ai.Ai_Trigger(collision))
            anim.SetBool("isDie", true);
    }

    public void Attack()
    {
        GameObject b = Instantiate(bullet, checkPos.position, Quaternion.identity);
        b.GetComponent<Bullet>().Shot(v2);
//        attack = false;
    }



}
