using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

namespace Game
{
	public class Mover : MonoBehaviour
	{
		[Tooltip("Player mouvement speed")]
		[SerializeField] private float speed;
		[Tooltip("Player Jump Speed")]
		[SerializeField] private float jumpSpeed;
		[Tooltip("Short period of time where the player can jump while being airborne.")]
		[SerializeField] private float playerNoLongerGroundedDelay;


		private KinematicRigidbody2D kinematicRigidbody2D;
		private Vector2 direction;
		private PlayerController player;
		
		private void Awake()
		{
			kinematicRigidbody2D = GetComponent<KinematicRigidbody2D>();
			direction = Vector2.right;
			player = GetComponent<PlayerController>();
		}

		private void Update()
		{
			var horizontalVelocity = Vector2.zero;
			var verticalVelocity = Vector2.zero;


			if (Input.GetKeyDown(KeyCode.Space) && kinematicRigidbody2D.TimeSinceAirborne < playerNoLongerGroundedDelay)
			{
				verticalVelocity = Vector2.up;
			}
				

			if (Input.GetKey(KeyCode.A))
			{
				horizontalVelocity += Vector2.left;
				direction = Vector2.left;
				player.DirectionFacingLeftRight = FacingSideLeftRight.Left;
				player.DirectionFacingUpDown = FacingSideUpDown.None;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				horizontalVelocity += Vector2.right;
				direction = Vector2.right;
				player.DirectionFacingLeftRight = FacingSideLeftRight.Right;
				player.DirectionFacingUpDown = FacingSideUpDown.None;
			}

			if (Input.GetKey(KeyCode.W))
			{
				player.DirectionFacingUpDown = FacingSideUpDown.Up;
				direction = Vector2.up;
			}

			if (Input.GetKey(KeyCode.S))
			{
				player.DirectionFacingUpDown = FacingSideUpDown.Down;
				direction = Vector2.down;
			}
			
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				if(player.CurrentController.CapacityUsable(player))
				player.CurrentController.UseCapacity(player,direction);
			}

			if (Input.GetKey(KeyCode.Return))
			{
				player.CurrentController.UseBasicAttack(player ,direction);
			}
			
				
			kinematicRigidbody2D.Velocity = horizontalVelocity * speed +verticalVelocity * jumpSpeed;
			
			
		}


		
	}
}
