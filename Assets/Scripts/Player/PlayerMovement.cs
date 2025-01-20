using UnityEngine;
using Managers;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        private PlayerControl _playerControl;
        public CharacterController characterController;
        public Animator animator;
        
        [Header("Movement")]
        [SerializeField]
        private Vector3 currentMovement;
        [SerializeField]
        private float rotationSpeed = 5f;
        [SerializeField]
        private float movementSpeed;
        private Vector2 _currentMovementInput;
        
        private bool _isMovementPressed;

        public VariableJoystick variableJoystick;

        private void Awake()
        {
            _playerControl = new PlayerControl();
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();

            AnimationManager.Instance.InitializeAnimators(animator, "isMoving");
            
            _playerControl.PlayerMovement.Movement.started += OnMovementInput;
            _playerControl.PlayerMovement.Movement.canceled += OnMovementInput;
            _playerControl.PlayerMovement.Movement.performed += OnMovementInput;
        }

        private void OnMovementInput(InputAction.CallbackContext ctx)
        {
            _currentMovementInput = ctx.ReadValue<Vector2>();
            currentMovement.x = _currentMovementInput.x;
            currentMovement.z = _currentMovementInput.y;
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        }

        private void OnMovementInputJoyStick()
        {
            Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            float x = variableJoystick.Vertical;
            currentMovement.x = x;
            float y = variableJoystick.Horizontal;
            currentMovement.z = y;
            _isMovementPressed = x != 0 || y != 0;
        }

        private void Update()
        {
            HandleMovement();
            HandleAnimation();
            if (_isMovementPressed)
            { 
                HandleRotation();
            }
        }

        private void HandleMovement()
        {
            Vector3 movement = currentMovement;
            movement.y = -1f;
            characterController.Move(movement * (movementSpeed * Time.deltaTime));
        }

        private void HandleRotation()
        {
            Vector3 direction;
            direction.x = currentMovement.x;
            direction.y = 0;
            direction.z = currentMovement.z;

            if (direction.sqrMagnitude > 0.01)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        private void HandleAnimation()
        {
            AnimationManager.Instance.SetBool(animator, "isMoving", _isMovementPressed);
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
