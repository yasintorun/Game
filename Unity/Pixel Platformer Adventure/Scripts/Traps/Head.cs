using UnityEngine;

public class Head : MonoBehaviour
{
    Animator anim;
    AIMovement ai;
    float BaseSpeed;
    private int dir = 0;

    [SerializeField]
    private Transform[] DirTransform;

    void Start()
    {
        ai = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();

        BaseSpeed = ai.speed;
    }
    bool dd  = true;

    public enum Type
    {
        Rock,
        Spike
    }
    public Type HeadType;

    void Update()
    {
        
        if (ai.GetIsMove())
        {
            anim.SetInteger("dir", 0);
            dir = ai.WhatIsDir();
            ai.speed += ai.speed * Time.deltaTime;
            dd = true; 
        }
        else if(dd) //sadece bir defa girsin.
        {
            dd = false;
            anim.SetInteger("dir", dir);
            ai.speed = BaseSpeed;
            if (HeadType == Type.Rock && dir > 0 && dir < 5)
            {
                Collider2D[] c = Physics2D.OverlapCircleAll(DirTransform[dir - 1].position, 0.2f);
                foreach (Collider2D cc in c)
                {
                    if (cc.gameObject.CompareTag("Player"))
                        cc.GetComponent<PlayerController>().PlayerDead();
                }
            }
        }
        ai.Move();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(HeadType == Type.Spike && collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            //GameManager.RestartScene();
            collision.GetComponent<PlayerController>().PlayerDead();
        }
    }

    public void AnimSetDir()
    {
        anim.SetInteger("dir", 0);
    }



}
