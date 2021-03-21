using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public Transform checkPos;

    public GameObject bulletPieces;

    Rigidbody2D rb;

    private Vector2 dir;
    private bool shot;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(shot)
        {
            rb.velocity = dir * speed * Time.deltaTime;
        }
    }

    public void Shot(Vector2 v2)
    {
        dir = v2;
        shot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(bulletPieces, transform.position, Quaternion.identity);

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
        }
        Destroy(gameObject);
    }

}
