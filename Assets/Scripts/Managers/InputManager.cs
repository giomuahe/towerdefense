using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControl _playerControl;
        public CharacterController characterController;
        
        private Vector2 _currentMovementInput;
        private Vector3 _currentMovement;
        private bool _isMovementPressed;

        private void Awake()
        {
            _playerControl = new PlayerControl();
            characterController = GetComponent<CharacterController>();
            
            _playerControl.PlayerMovement.Movement.started += ctx =>
            {
                _currentMovementInput = ctx.ReadValue<Vector2>();
                _currentMovement.x = _currentMovementInput.x;
                _currentMovement.y = _currentMovementInput.y;
                _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
            };
        }

        private void OnEnable()
        {
            _playerControl.PlayerMovement.Enable();
        }

        private void OnDisable()
        {
            _playerControl.PlayerMovement.Disable();
        }
    }
}
