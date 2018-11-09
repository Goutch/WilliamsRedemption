using UnityEngine;
using XInputDotNetPure;

namespace Game.Entity.Player
{
    public class PlayerInputs : MonoBehaviour
    {
        private Mover player;
        private PlayerIndex controllerNumber;
        private PlayerController playerController;
        private GamePadState controllerState;
        private bool jumpButtonPressed;

        void Start()
        {
            player = GetComponent<Mover>();
            playerController = GetComponent<PlayerController>();
            controllerNumber = PlayerIndex.One;
            controllerState = GamePad.GetState(controllerNumber);
            jumpButtonPressed = false;
        }

        void Update()
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

            if (Input.GetKeyDown(KeyCode.Space) && !PlayerController.instance.IsStun)
            {
                player.Jump();
            }

            if (Input.GetKey(KeyCode.A) && !PlayerController.instance.IsStun)
            {
                player.MoveLeft();
            }
            else if (Input.GetKey(KeyCode.D) && !PlayerController.instance.IsStun)
            {
                player.MoveRight();
            }
            else
            {
                playerController.IsMoving = false;
            }

            if (Input.GetKey(KeyCode.W))
            {
                player.AimUp();
            }

            if (Input.GetKey(KeyCode.S))
            {
                player.AimDown();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && playerController.CurrentController.CapacityUsable(playerController) && !PlayerController.instance.IsStun)
            {
                playerController.CurrentController.UseCapacity(playerController);
            }

            if (Input.GetKey(KeyCode.Return) && playerController.CurrentController.CanUseBasicAttack() && !PlayerController.instance.IsStun)
            {
                playerController.CurrentController.UseBasicAttack(playerController);
            }

        }

        private void ManageControllerInputs()
        {
            controllerState = GamePad.GetState(controllerNumber);

            if (controllerState.Buttons.A == ButtonState.Pressed && !jumpButtonPressed && !PlayerController.instance.IsStun)
            {
                player.Jump();
                jumpButtonPressed = true;

            }
            if (controllerState.Buttons.A == ButtonState.Released && !PlayerController.instance.IsStun)
            {
                jumpButtonPressed = false;
                playerController.IsMoving = false;
            }

            if (controllerState.ThumbSticks.Left.X <= -0.5 && !PlayerController.instance.IsStun)
            {
                player.MoveLeft();
            }
            else if (controllerState.ThumbSticks.Left.X >= 0.5 && !PlayerController.instance.IsStun)
            {
                player.MoveRight();
            }
            else
            {
                playerController.IsMoving = false;
            }

            if (controllerState.ThumbSticks.Left.Y >= 0.5)
            {
                player.AimUp();
            }

            if (controllerState.ThumbSticks.Left.Y <= -0.5)
            {
                player.AimDown();
            }

            if (controllerState.Buttons.X == ButtonState.Pressed && playerController.CurrentController.CapacityUsable(playerController) && !PlayerController.instance.IsStun)
            {
                playerController.CurrentController.UseCapacity(playerController);
            }

            if (controllerState.Buttons.B == ButtonState.Pressed && playerController.CurrentController.CanUseBasicAttack() && !PlayerController.instance.IsStun)
            {
                playerController.CurrentController.UseBasicAttack(playerController);
            }

        }
    }
}

