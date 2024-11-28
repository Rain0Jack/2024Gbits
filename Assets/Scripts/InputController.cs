using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>
{
    public PlayerInputControl playerInputControl;
    private InputController()
    {
        playerInputControl = new PlayerInputControl();
        playerInputControl.GamePlay.Enable();
    }
}
