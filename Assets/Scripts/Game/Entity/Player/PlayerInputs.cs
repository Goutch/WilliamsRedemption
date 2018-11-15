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


            if (Input.GetKeyDown(KeyCode.LeftShift) &&
                playerController.CurrentController.CapacityUsable(playerController))
            {
                playerController.CurrentController.UseCapacity(playerController);
            }

            if (Input.GetKey(KeyCode.Return) && playerController.CurrentController.CanUseBasicAttack())
            {
                playerController.CurrentController.UseBasicAttack(playerController);
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
            }

            if (controllerState.ThumbSticks.Left.X <= -0.5)
            {
                player.MoveLeft();
            }
            else if (controllerState.ThumbSticks.Left.X >= 0.5)
            {
                player.MoveRight();
            }

            if (controllerState.Buttons.X == ButtonState.Pressed &&
                playerController.CurrentController.CapacityUsable(playerController))
            {
                playerController.CurrentController.UseCapacity(playerController);
            }

            if (controllerState.Buttons.B == ButtonState.Pressed &&
                playerController.CurrentController.CanUseBasicAttack())
            {
                playerController.CurrentController.UseBasicAttack(playerController);
            }
        }
    }
}