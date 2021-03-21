using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : MonoBehaviour
{
    AIMovement ai;
    void Start()
    {
        ai = GetComponent<AIMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
        ai.Move(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ai.Ai_Collision(collision))
        {
            ai.speed = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ai.Ai_Trigger(collision))
        {
            GetComponent<Animator>().SetBool("hit", true);
        }
    }
    public void Die()
    {
        GetComponent<Animator>().enabled = false;
    }
}
