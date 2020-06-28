using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private InputMaster inputMaster;
    [SerializeField] private ActiveRagdollLegContoller movement2D;
    [SerializeField] private ActiveRagdollArmController arms;
    private Vector2 mousePos;
    private enum Player
    {
        PLAYER_1,
        PLAYER_2
    }
    [SerializeField] private Player player;

    private void Awake()
    {
        inputMaster = new InputMaster();

        if (player == Player.PLAYER_1)
        {
            inputMaster.Player1.Move.performed += ctx => movement2D.InputMove = ctx.ReadValue<float>();
            inputMaster.Player1.Move.canceled += ctx => movement2D.InputMove = 0;

            inputMaster.Player1.Jump.performed += ctx => movement2D.Jump();

            inputMaster.Player1.Aim.performed += ctx => arms.Aim = ctx.ReadValue<Vector2>();
            
        }
        if (player == Player.PLAYER_2)
        {
            inputMaster.Player2.Move.performed += ctx => movement2D.InputMove = ctx.ReadValue<float>();
            inputMaster.Player2.Move.canceled += ctx => movement2D.InputMove = 0;

            inputMaster.Player2.Jump.performed += ctx => movement2D.Jump();

            inputMaster.Player2.Aim.performed += ctx => arms.Aim = ctx.ReadValue<Vector2>();
        }
        
    }

    private void OnEnable()
    {
        if (player == Player.PLAYER_1)
            inputMaster.Player1.Enable();
        else
            inputMaster.Player2.Enable();
    }
}
