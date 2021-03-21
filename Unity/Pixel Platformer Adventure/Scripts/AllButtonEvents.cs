using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AllButtonEvents : MonoBehaviour
{
    PlayerInput player;

    private void Start()
    {
        player = FindObjectOfType<PlayerInput>();
    }
    public void JumpButtonPressed()
    {
        player.JumpButtonPressed();
    }

    public void MoveButtonPressed(float h)
    {
        player.MoveButtonPressed(h);
    }


    public void CloseWindowAnimationEvent(int i = 0)
    {
        if (i == 1)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(false);
    }


    public void BackHome()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }




}
