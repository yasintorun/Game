using UnityEngine;

public class Fire : MonoBehaviour
{
    Animator anim;
    bool isTouch = false;
    float timer = 0f;
    public float delay = 0.4f;

    public Collider2D fireField;
    private Collider2D player;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(isTouch)
        {
            if (fireField.enabled && fireField.IsTouching(player))
            {
                fireField.enabled = false;
                player.gameObject.GetComponent<PlayerController>().PlayerDead();
            }
            else
            {
                timer += Time.fixedDeltaTime;
                if (timer > delay)
                {
                    if (anim.GetInteger("index") == 1)
                    {
                        fireField.enabled = true;
                        anim.SetInteger("index", 2);
                        delay = 1f;
                    }
                    else
                    {

                        fireField.enabled = false;
                        delay = 0.4f;
                        anim.SetInteger("index", 0);
                        isTouch = false;
                    }
                    timer = 0f;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            isTouch = true;
            anim.SetInteger("index", 1);
            player = collision;
        }
    }

}
