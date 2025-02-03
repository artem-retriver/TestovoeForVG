using UnityEngine;

namespace Player
{
    public class PlayerMoveController : MonoBehaviour
    {
        [Header("Properties")]
        public float speed = 5f;
        public float jumpForce = 5f;
        public float mouseSensitivity = 2f;
    
        private Camera _playerCamera;
        private CharacterController _controller;
        private Rigidbody _rigidbody;
        private Vector3 _moveDirection;
        private float _verticalRotation;

        private void Start()
        {
            _playerCamera = Camera.main;
            _controller = GetComponent<CharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                _rigidbody.isKinematic = true;
                return;
            }
            
            MovePlayer();
            RotatePlayer();
        }
    
        private void MovePlayer()
        {
            _rigidbody.isKinematic = false;
            
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
        
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
            if (_controller.isGrounded)
            {
                _moveDirection = move * speed;
                if (Input.GetButtonDown("Jump"))
                {
                    _moveDirection.y = jumpForce;
                }
            }
        
            _moveDirection.y += Physics.gravity.y * Time.deltaTime;
            _controller.Move(_moveDirection * Time.deltaTime);
        }

        private void RotatePlayer()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
            transform.Rotate(Vector3.up * mouseX);
            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
            _playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }
    }
}
