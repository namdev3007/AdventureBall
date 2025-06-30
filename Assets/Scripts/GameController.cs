using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController _playerCl;

    private float xHorizontal;

#if UNITY_EDITOR
    void Update()
    {
        xHorizontal = Input.GetAxisRaw("Horizontal");
        if (xHorizontal == 1)
        {
            ClickRight();
        }
        else if (xHorizontal == -1)
        {
            ClickLeft();
        }
        else
        {
            ExitButton();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ClickJump();
        }
        else
        {
            ExitJump();
        }
    }
#endif

    public void ClickLeft()
    {
        _playerCl.UpdateMovementLeft();
    }

    public void ClickRight()
    {
        _playerCl.UpdateMovementRight();
    }

    public void ClickJump()
    {
        _playerCl.UpdateJumpping();
    }

    public void ExitJump()
    {
        _playerCl.NotJumpping();
    }

    public void ExitButton()
    {
        _playerCl.UpdateMovementUp();
    }
}