using TMPro;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float DestroyTime = 1f;
    public float speed = 0.5f;
    Vector3 StartPos, newPos;
    Animator anim;
    public GameObject effect;
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartPos = transform.position;
        newPos = new Vector3(0, 0.15f, 0);
    }
    bool isTouch = false;
    float timer = 0f;
    private void Update()
    {
        if (isTouch)
        {
            timer += Time.deltaTime;
            if (timer > DestroyTime)
            {
                effect.SetActive(false);
                GetComponent<BoxCollider2D>().isTrigger = true;
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(0, -100, 0), 10 * Time.deltaTime);
            }
            if (transform.position.y < -10)
                Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, StartPos + newPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, StartPos + newPos) < 0.05f)
            {
                newPos *= -1;
            }
        } 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            if (!collision.gameObject.GetComponent<PlayerController>().isGrounded) return;

            anim.SetTrigger("isTouch");
            isTouch = true;
        }
    }
}
