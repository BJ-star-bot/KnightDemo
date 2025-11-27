using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerAction
{
    MoveForward,
    MoveLeft,
    MoveRight,
    MoveBack,
    Jump,
    RunForward,
    RunLeft,
    RunRight,
    RunBack,
Attack
}
public class InputManager : MonoBehaviour
{
    private IInputReceiver inputReceiver;
    private Dictionary<PlayerAction, KeyCode> inputMap;
    void Start()
    {
        inputMap = new Dictionary<PlayerAction, KeyCode>
        {
            {PlayerAction.MoveForward,KeyCode.W},

        };
    }

    void Update()
    {
        foreach (var action in inputMap)
        {
            if (Input.GetKey(action.Value))
            {
                inputReceiver.OnActionTriggered(action.Key);
            }
        }
    }
}
public interface IInputReceiver
{
    public void OnActionTriggered(PlayerAction playerAction);
    
}