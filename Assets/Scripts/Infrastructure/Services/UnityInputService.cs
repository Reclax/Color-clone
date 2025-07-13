using ColorClone.Domain.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ColorClone.Infrastructure.Services
{
    public class UnityInputService : IInputService
    {
        private readonly InputAction _jump;
        private readonly InputAction _pause;

        public UnityInputService()
        {
            _jump = new InputAction("Jump", InputActionType.Button);
            _jump.AddBinding("<Keyboard>/space");
            _jump.AddBinding("<Gamepad>/buttonSouth");
            _jump.Enable();

            _pause = new InputAction("Pause", InputActionType.Button);
            _pause.AddBinding("<Keyboard>/escape");
            _pause.AddBinding("<Gamepad>/start");
            _pause.Enable();
        }

        public bool GetJumpDown()
        {
            return _jump.WasPressedThisFrame();
        }

        public bool GetPauseDown()
        {
            return _pause.WasPressedThisFrame();
        }
    }
}