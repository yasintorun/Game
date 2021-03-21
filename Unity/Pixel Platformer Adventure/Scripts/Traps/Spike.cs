using UnityEngine;

public class Spike : MonoBehaviour
{

    void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<SpriteRenderer>().size.x, GetComponent<BoxCollider2D>().size.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            //GameManager.RestartScene();
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
        }
    }

}
