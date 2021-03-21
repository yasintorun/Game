using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Range(1f, 5f)]
    public float ghostTime = 2f;
    private float timer;
    private bool ghost = false;
    AIMovement ai;
    Animator anim;
    void Start()
    {
        ai = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > ghostTime)
        {
            ghost = !ghost;
            anim.SetBool("ghost", ghost);
            timer = ghost ? timer / 2 : 0f;
        }

        ai.Move(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ai.Ai_Collision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( ai.Ai_Trigger(collision) )
        {
            anim.SetBool("hit", true);
            enabled = false;
        }
    }
    private void LateUpdate()
    {
        if(anim.GetBool("hit"))
            GetComponent<BoxCollider2D>().enabled = false;
    }

}
