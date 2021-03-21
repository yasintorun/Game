using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator anim;
    AIMovement ai;
    void Start()
    {
        anim = GetComponent<Animator>();
        ai = GetComponent<AIMovement>();
    }

    void Update()
    {
        if (!ai.enabled) return;
        ai.Move(true);
        /* if (ai.GetIsMove())
         {
             anim.SetBool("run", false);
         }
         else
             anim.SetBool("run", true);*/
        anim.SetBool("run", ai.GetIsMove());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ai.Ai_Collision(collision)) anim.SetBool("run", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ai.Ai_Trigger(collision)) anim.SetBool("hit", true);
    }



}
