using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInputs : MonoBehaviour
{
	private Mover player;
	private PlayerIndex controllerNumber;
	private PlayerController playerController;
	private GamePadState controllerState;
	private bool jumpButtonPressed;

	void Start ()
	{
		player = GetComponent<Mover>();
		playerController = GetComponent<PlayerController>();
		controllerNumber = PlayerIndex.One;
		controllerState = GamePad.GetState(controllerNumber);
		jumpButtonPressed = false;
	}
	
	void Update () 
	{
		if (!controllerState.IsConnected)
		{
			ManageKeyBoardInputs();
		}
		else
		{
			ManageControllerInputs();
		}
	}

	private void ManageKeyBoardInputs()
	{
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			player.Jump();
		}
				
		if (Input.GetKey(KeyCode.A))
		{
			player.MoveLeft();
		}
		else if (Input.GetKey(KeyCode.D))
		{
			player.MoveRight();
		}
		else
		{
			playerController.IsMoving = false;
		}

		if (Input.GetKey(KeyCode.W ))
		{
			player.AimUp();
		}

		if (Input.GetKey(KeyCode.S))
		{
			player.AimDown();
		}
			
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.UseCapacity();
		}

		if (Input.GetKey(KeyCode.Return)&&PlayerController.instance.CurrentController.CanUseBasicAttack())
		{
			player.Attack();
		}
					
	}
	
	private void ManageControllerInputs()
	{
		controllerState = GamePad.GetState(controllerNumber);
			
		if (controllerState.Buttons.A == ButtonState.Pressed && !jumpButtonPressed)
		{
			player.Jump();
			jumpButtonPressed = true;
				
		}
		if (controllerState.Buttons.A == ButtonState.Released)
		{
			jumpButtonPressed = false;
			playerController.IsMoving = false;
		}

		if (controllerState.ThumbSticks.Left.X <= -0.5)
		{
			player.MoveLeft();
		}
		else if (controllerState.ThumbSticks.Left.X >=0.5)
		{
			player.MoveRight();
		}
		else
		{
			playerController.IsMoving = false;
		}

		if (controllerState.ThumbSticks.Left.Y >=0.5)
		{
			player.AimUp();
		}

		if (controllerState.ThumbSticks.Left.Y <= -0.5)
		{
			player.AimDown();
		}
			
		if (controllerState.Buttons.X == ButtonState.Pressed)
		{
			player.UseCapacity();
		}

		if (controllerState.Buttons.B == ButtonState.Pressed)
		{
			player.Attack();
		}
				
	}
	
	
	
}
