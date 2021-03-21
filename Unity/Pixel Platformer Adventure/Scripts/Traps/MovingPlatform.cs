using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public enum Type
    {
        Brawn,
        Grey
    }
    public Type PlatformType; 


    AIMovement ai;
    Animator anim; 
    bool istouch = false;
    void Start()
    {
        ai = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (PlatformType == Type.Brawn)
        {
            if (istouch)
            {
                if (Vector3.Distance(transform.position, ai.moveSpots[1].position) < 0.1f)
                {
                    anim.SetBool("isTouch", false);
                    return;
                }
                anim.SetBool("isTouch", true);
                transform.position = Vector2.MoveTowards(transform.position, ai.moveSpots[1].position, ai.speed * Time.deltaTime);


            } else
            {
                if (Vector3.Distance(transform.position, ai.moveSpots[0].position) < 0.1f)
                {
                    anim.SetBool("isTouch", false);
                    return;
                }
                anim.SetBool("isTouch", true);
                transform.position = Vector2.MoveTowards(transform.position, ai.moveSpots[0].position, ai.speed * Time.deltaTime);
            } 
        }
        else
        {
            ai.Move();
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            istouch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            istouch = false;
        }
    }
}
