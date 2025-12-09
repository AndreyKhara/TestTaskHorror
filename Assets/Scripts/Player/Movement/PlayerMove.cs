using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

    public class PlayerMove : MonoBehaviour
    {
        public float MoveSpeed = 5f;
        public float JumpForce = 5f;
        public LayerMask Ground;
 
        [SerializeField] private Rigidbody _rgb;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private float _moveCameraAnimationAmount = 0.7f; // Смещение от базовой позиции
        [SerializeField] private float _durationAnimation = 0.5f;
        [SerializeField] private PlayerInput _playerInput;

        private bool _onGround;
        private InputAction _move;
        private InputAction _jump;
        private Vector3 _moveDirection3D;
        private Vector2 _inputVector; //Сохраняем значение ввода

        private Sequence _animationSequence;
        private Vector3 _cameraBasePosition;


        private void Awake()
        {
            _move = _playerInput.actions["Move"];
            _jump = _playerInput.actions["Jump"];


            _move.performed += OnMovePerformed;
            _move.canceled += OnMoveCanceled;

            _jump.performed += JumpAction;

            _cameraBasePosition = _cameraTransform.localPosition;
        }

        private void OnEnable()
        {
            _move.Enable();
        }

        private void OnDisable()
        {
            _move.performed -= OnMovePerformed;
            _move.canceled -= OnMoveCanceled;

            _move.Disable();
        }


        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            //Debug.Log("MOve Perfomed");
            _inputVector = context.ReadValue<Vector2>();
            StartAnimationPlayer();
            UpdateMoveDirection();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            //Debug.Log("Move Canceled");
            _inputVector = Vector2.zero;
            StopAnimationPlayer();
            UpdateMoveDirection();

        }


        private void UpdateMoveDirection()
        {
            Vector3 playerForward = transform.forward;
            playerForward.y = 0;
            playerForward.Normalize();


            Vector3 playerRight = transform.right;
            playerRight.y = 0;
            playerRight.Normalize();

            _moveDirection3D = (playerForward * _inputVector.y + playerRight * _inputVector.x).normalized;
        }

        private void JumpAction(InputAction.CallbackContext context)
        {
            _onGround = Physics.OverlapSphere(_groundChecker.position, 0.5f, Ground).Length > 0;
           // Debug.Log("jump");
            if (_onGround)
            {
                StopAnimationPlayer();
                _rgb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);

            }
        }

        private void FixedUpdate()
        {
            UpdateMoveDirection();
            Vector3 movement = _moveDirection3D * MoveSpeed * Time.fixedDeltaTime;
            _rgb.AddForce(movement, ForceMode.VelocityChange);
        }

        private void StartAnimationPlayer()
        {
            _animationSequence?.Kill();
            _animationSequence = DOTween.Sequence();

            _animationSequence.Append(
                _cameraTransform.DOLocalMoveY(_cameraBasePosition.y - _moveCameraAnimationAmount, _durationAnimation)
                .SetEase(Ease.Linear)
            );

            _animationSequence.Append(
                _cameraTransform.DOLocalMoveY(_cameraBasePosition.y + _moveCameraAnimationAmount, _durationAnimation)
                .SetEase(Ease.Linear)
            );

            _animationSequence.SetLoops(-1, LoopType.Yoyo);  
        }

        private void StopAnimationPlayer()
        {
            _animationSequence?.Kill();
            _cameraTransform.DOLocalMove(_cameraBasePosition, _durationAnimation);

        }

        private void OnDestroy()
        {
            _animationSequence.Kill();
        }
    }
