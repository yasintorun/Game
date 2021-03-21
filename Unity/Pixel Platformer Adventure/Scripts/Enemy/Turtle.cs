using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float conversionTime = 1;
    public float startTimePoint = 0;

    float timer;
    bool spikes = false;
    Animator anim;
    void Start()
    {
        timer = startTimePoint + Random.Range(0, conversionTime - startTimePoint); // levele başlandıgında hep farklı zamandan başlasın.
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > conversionTime)
        {
            anim.SetTrigger(spikes ? "spikes_in" : "spikes_out");
            spikes = !spikes;
            timer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !anim.GetBool("isDie"))
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFoot"))
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            this.enabled = false;
            if (spikes)
            {
                collision.transform.parent.gameObject.GetComponent<PlayerController>().PlayerDead();
                return;
            }
            collision.transform.parent.GetComponent<PlayerController>().JumpPlayer();
            anim.SetBool("isDie", true);
        }
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().velocity = Vector2.up * Random.Range(1, 4);
    }



}
