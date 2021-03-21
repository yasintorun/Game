using UnityEngine;

public class Saw : MonoBehaviour
{
    AIMovement ai;
    void Start()
    {
        ai = GetComponent<AIMovement>();
    }

    void FixedUpdate()
    {
        ai.Move();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            //GameManager.RestartScene();
            collision.gameObject.GetComponent<PlayerController>().PlayerDead();
        }
    }
}
