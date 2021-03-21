using UnityEngine;

public class Radish : MonoBehaviour
{

    public float waitTime;
    AIMovement ai;

    public Transform[] groundSpot;

    public GameObject leafsPrefab;

    Animator anim;

    bool isFirstHit = false;
    void Start()
    {
        ai = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(isFirstHit)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            ai.moveSpots = groundSpot;
            ai.speed *= 2;
            ai.SetIndex(0);
            ai.SetTime(waitTime);
            isFirstHit = false;
            //isDie = false;
            anim.SetBool("hit", true);
            Instantiate(leafsPrefab, transform.position, Quaternion.identity);
        }
        else if(anim.GetBool("hit") && GetComponent<Rigidbody2D>().velocity.y < 0.1f)
        {
           transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("hit", false);
        }
        ai.Move(true);
        anim.SetBool("run", ai.GetIsMove());
    }
    bool isDie = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player") isDie = false;

        if (!isDie)
        {
            ai.Ai_Collision(collision);
            isDie = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDie && !isFirstHit && collision.gameObject.CompareTag("PlayerFoot"))
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            isFirstHit = true;
            isDie = true;
            return;
        }
        else if (ai.Ai_Trigger(collision))
        {
            anim.SetBool("isDie", true);
            isDie = true;
            enabled = false;
        }
    }

}
