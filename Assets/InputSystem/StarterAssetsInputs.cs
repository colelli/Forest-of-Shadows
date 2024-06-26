using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{

		public event EventHandler OnInteractAction;
		public static event EventHandler OnEscapePressed;

		[Header("Character Input Values")]
		private Player _player;
		public Vector2 move;
		public Vector2 look;
		public bool jump;
        private float _jumpStaminaCost = 10f;
        public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private void Start() {
            _player = GetComponent<Player>();
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value) {
			InteractInput(value.isPressed);
		}

		public void OnEscape(InputValue value) {
			EscapeInput(value.isPressed);
		}

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (_player.HasEnoughStamina(_jumpStaminaCost)) {
				jump = newJumpState;
				_player.DecreaseStamina(_jumpStaminaCost);
			}
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState) {
			if (newInteractState) {
				OnInteractAction?.Invoke(this, EventArgs.Empty);
			}
		}

		public void EscapeInput(bool newEscapeState) {
			Debug.Log("Esc Pressed");
			if (newEscapeState) {
				OnEscapePressed?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}