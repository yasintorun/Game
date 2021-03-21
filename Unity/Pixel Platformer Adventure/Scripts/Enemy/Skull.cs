using UnityEngine;

public class Skull : MonoBehaviour
{

    public float speed;
    void Start()
    {
        float dirx = Random.Range(1, 3) == 1 ? 1 : -1; //%50 ihtimal.
        float diry = Random.Range(1, 3) == 1 ? 1 : -1;
        Vector2 v2 = new Vector2(dirx,diry) * speed;
        GetComponent<Rigidbody2D>().AddForce(v2);

    }

    bool hit = false;

    /*private void FixedUpdate()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, col.radius);
        foreach(Collider2D c in cols)
        {
            if(c.tag == "Platform")
            {
                Debug.Log(Vector2.Reflect(v2, c.transform.position));
                v2 = Vector2.Reflect(v2,Vector2.up);
            }
        }

        transform.Translate(v2*speed*Time.fixedDeltaTime);
    }
    */

    bool isDie = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("Player") && !isDie)
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
            //GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //enabled = false;
            isDie = true;
            return;
        }

        if (collision.gameObject.layer == 8) //ground -> platform
        {
            GetComponent<Animator>().SetTrigger(hit ? "hit2" : "hit1");
            hit = !hit;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 2));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerFoot") && !isDie)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            if (hit) return;
            isDie = true;
            Debug.Log("asd");
            collision.transform.parent.gameObject.GetComponent<PlayerController>().JumpPlayer();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>().SetBool("isDie", true);
        }
    }
    public void Die()
    {
        this.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SurfaceEffector2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 3;
        //GetComponent<Rigidbody2D>().velocity = Vector2.up * Random.Range(2, 5);
    }


}
