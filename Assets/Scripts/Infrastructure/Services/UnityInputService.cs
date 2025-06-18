using ColorClone.Domain.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ColorClone.Infrastructure.Services
{
    public class UnityInputService : IInputService
    {
        private readonly InputAction _jump;

        public UnityInputService()
        {
            _jump = new InputAction("Jump", InputActionType.Button);
            _jump.AddBinding("<Keyboard>/space");
            _jump.AddBinding("<Gamepad>/buttonSouth");
            _jump.Enable();
        }

        public bool GetJumpDown()
        {
            return _jump.WasPressedThisFrame();
        }
    }
}