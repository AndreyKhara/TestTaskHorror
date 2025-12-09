using UnityEngine;
using UnityEngine.InputSystem;

    public class PlayerCameraController : MonoBehaviour
    {   

        [SerializeField] private Transform _cameraTransform;

        [Header("Настройки вращения")]
        [SerializeField] private float sensitivity = 2.0f; // Чувствительность вращения
        [SerializeField] private float smoothing = 2.0f;   // Сглаживание вращения
        [SerializeField] private float minPitch = -90f;
        [SerializeField] private float maxPitch = 90f;
        [SerializeField] private PlayerInput _playerInput;

        private InputAction _cameraControl;
        private Transform _selfTransform;

        private Vector2 _currentMouseDelta;  // Текущее смещение мыши
        private Vector2 _smoothedMouseDelta; // Сглаженное смещение мыши
        private Vector2 _cameraRotation;     // Накопленные углы вращения камеры (x для Yaw, y для Pitch)

        private void Awake()
        {
            _cameraControl = _playerInput.actions["CameraControl"];

            _selfTransform = GetComponent<Transform>();

            _cameraControl.performed += OnCameraControlPerformed;
            _cameraControl.performed += OnCameraControlCanceled;

        }

        private void OnEnable()
        {
            _cameraControl.Enable();
        }


        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        private void OnCameraControlPerformed(InputAction.CallbackContext context)
        {

            _currentMouseDelta = context.ReadValue<Vector2>();

            // Применяем чувствительность
            _currentMouseDelta *= sensitivity;

            // Применяем сглаживание к дельте мыши
            _smoothedMouseDelta.x = Mathf.Lerp(_smoothedMouseDelta.x, _currentMouseDelta.x, 1f / smoothing);
            _smoothedMouseDelta.y = Mathf.Lerp(_smoothedMouseDelta.y, _currentMouseDelta.y, 1f / smoothing);

            // Накапливаем углы вращения
            _cameraRotation.x += _smoothedMouseDelta.x; // Yaw (вращение вокруг Y оси, горизонтальное)
            _cameraRotation.y += _smoothedMouseDelta.y; // Pitch (вращение вокруг X оси, вертикальное)

            // Ограничиваем вертикальное вращение (Pitch), чтобы камера не переворачивалась
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, minPitch, maxPitch);

            // Применяем горизонтальное вращение (Yaw) к родительскому объекту (игроку)
            _selfTransform.localRotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);

            // Применяем вертикальное вращение (Pitch) к Transform самой камеры
            _cameraTransform.localRotation = Quaternion.Euler(-_cameraRotation.y, 0f, 0f);


        }

        private void OnCameraControlCanceled(InputAction.CallbackContext context)
        {
            _currentMouseDelta = Vector2.zero;
            _smoothedMouseDelta = Vector2.zero;
        }


        private void OnDisable()
        {
            _cameraControl.performed -= OnCameraControlPerformed;

            _cameraControl.Disable();
        }

    }

