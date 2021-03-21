using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void EventHitAnim()
    {
        GetComponent<Animator>().SetBool("hit", false);
    }
}
