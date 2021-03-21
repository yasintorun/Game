using UnityEngine;

public class Slime : MonoBehaviour
{

    public GameObject slimeParticle;
    public Transform particleCheck;
    AIMovement ai;
    void Start()
    {
        ai = GetComponent<AIMovement>();
    }
    float timer = 0f;
    public float delay;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay)
        {
            GameObject p = Instantiate(slimeParticle, particleCheck.position, Quaternion.identity);
            Destroy(p, 5);
            timer = 0f;
        }

        ai.Move(true);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ai.Ai_Collision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ai.Ai_Trigger(collision))
            GetComponent<Animator>().SetBool("isDie", true);
    }

}
