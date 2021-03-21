using System.Text;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float TimeOpenFan;
    public float FanStopTime;
    float timer = 0f;
    Animator anim;
    public ParticleSystem effect;
    void Start()
    {
        timer = TimeOpenFan;
        anim = GetComponent<Animator>();
        GetComponent<AreaEffector2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        effect.Stop();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > FanStopTime)
        {
            bool on = !anim.GetBool("on");
            anim.SetBool("on", on);
            timer = 0f;
            GetComponent<BoxCollider2D>().enabled = on;
            GetComponent<AreaEffector2D>().enabled = on;
            //effect.SetActive(on);
            if (on)
                effect.Play();
            else
                effect.Stop();
        }
    }
}
