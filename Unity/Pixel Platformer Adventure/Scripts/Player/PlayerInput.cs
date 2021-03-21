using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public bool isAndroid = false;
    PlayerController controller;

    private bool isJump = false;
    private float moving_x_axis;


    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public void JumpButtonPressed()
    {
        isJump = true;
    }

    public void MoveButtonPressed(float h)
    {
        moving_x_axis = h;
    }


    void Update()
    {
#if UNITY_EDITOR || UNITY_WIN
        if(!isJump)
            isJump = Input.GetKeyDown(KeyCode.W);
#endif

    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        moving_x_axis = Input.GetAxisRaw("Horizontal"); // for pc
#endif
        controller.Move(moving_x_axis, isJump);
        isJump = false;
    }

}
