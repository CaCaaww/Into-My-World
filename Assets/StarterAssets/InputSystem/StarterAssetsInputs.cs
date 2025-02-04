using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [SerializeField]
        private InteractEventChannel interactEventChannel;
        public ToggleDebugEventChannel toggleDebugEventChannel;

        [SerializeField] private GenericEventChannelSO<ToggleInventoryEvent> toggleInventoryEventChannel;
        [SerializeField] private GenericEventChannelSO<ResetEvent> resetEventChannel;
        private bool inventoryOpen = false;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
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

        public void OnClick(InputValue value)
        {
            interactEventChannel.RaiseEvent();
        }

        public void OnToggleDebug(InputValue value)
        {
            toggleDebugEventChannel.RaiseEvent();
        }
        public void OnToggleInventory(InputValue value) {
            inventoryOpen = !inventoryOpen;
            toggleInventoryEventChannel.RaiseEvent(new ToggleInventoryEvent(inventoryOpen));
        }
        public void OnReset(InputValue value) {
            resetEventChannel.RaiseEvent(new ResetEvent());
        }
        public delegate void OnClickDelegate();
        public delegate void OnToggleDebugDelegate();
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
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
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