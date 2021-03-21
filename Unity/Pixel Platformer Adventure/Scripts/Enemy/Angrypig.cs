using System.Threading;
using UnityEngine;

public class Angrypig : MonoBehaviour
{
    [Range(1.5f, 3f)]
    public float angrySpeed = 2; //yeşilden kırmızıya döndükten sonra hızı kaç katına çıksın?
    AIMovement ai;
    Animator anim;
    void Start()
    {
        ai = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();
    }
    bool GreenDie = false;
    float timer;
    void Update()
    {
        if (ai.enabled)
        {
            anim.SetBool("isWalk", ai.GetIsMove());
            ai.Move(true);
        }
        if(GreenDie && isDie)
        {
            timer += Time.deltaTime;
            if (timer > .5f)
                isDie = false;
        }
    }

    bool isDie = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDie && ai.Ai_Trigger(collision, GreenDie))
        {
            isDie = true;
            if(!GreenDie)
            {
                GreenDie = true;
                ai.SetEnemyDie(false);
                anim.SetTrigger("hit");
                ai.speed *= angrySpeed;
                ai.SetTime(0);
            }else
            {
                anim.SetBool("isDie", true);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isDie)
            ai.Ai_Collision(collision);    
    }
}
