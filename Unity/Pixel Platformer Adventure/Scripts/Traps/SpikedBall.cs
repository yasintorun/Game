using UnityEditor;
using UnityEngine;

public class SpikedBall : MonoBehaviour
{
    public float speed = 1f;
    public enum Type
    {
        _180Derece,
        _360Derece
    }
    public Type donusTipi;

    void FixedUpdate()
    {
        if (donusTipi == Type._180Derece)
        {
            float z = transform.rotation.eulerAngles.z;
            if(z < 270 && z > 90)
            {
                speed *= -1;
            }
        }
        transform.Rotate(new Vector3(0f, 0f, speed), Space.Self);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerInput>().enabled)
        {
            //GameManager.RestartScene();
            collision.GetComponent<PlayerController>().PlayerDead();
        }
    }

}
