using Deforestation;
using System;
using UnityEngine;

namespace Deforastation.Player
{
	public class PlayerController : MonoBehaviour
	{
		#region Propierties
		public HealthSystem HealthSystem => _health;
		public Action<bool> OnCollisionWaterPlayer;
		#endregion

		#region Fields
		[Header("Movement")]
		[SerializeField] private float _moveSpeed = 5f;
		[SerializeField] private float _runSpeed = 9f;
		[SerializeField] private float _jumpForce = 6f;

		[Header("Ground Check")]
		[SerializeField] private float _groundCheckDistance = 1.1f;
		[SerializeField] private LayerMask _groundMask;

		[Header("Mouse Look")]
		[SerializeField] private float _mouseSensitivity = 140f;
		[SerializeField] private Transform _cameraTransform;

		private HealthSystem _health;
		private CharacterController _characterController;
		private Vector3 _velocity;
		private bool _isGrounded;
		private float _xRotation;
		#endregion

		#region Unity CallBacks
		private void Start()
		{
			_characterController = GetComponent<CharacterController>();
			_health = GetComponent<HealthSystem>();
		}
		private void Update()
		{
			CheckGround();
			Look();
			Move();
			Jump();
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Water"))
				GameController.Instance.OnWarningPanelOn?.Invoke(true);
			if (other.CompareTag("Final"))
				GameController.Instance.OnFinalGame?.Invoke();
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Water"))
				GameController.Instance.OnWarningPanelOn?.Invoke(false);
		}
		#endregion

		#region Private Methods
		private void CheckGround()
		{
			_isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundMask);

			if (_isGrounded && _velocity.y < 0f)
				_velocity.y = 0f;
		}

		private void Look()
		{
			if (_cameraTransform == null)
				return;

			float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

			transform.Rotate(0f, mouseX, 0f);

			_xRotation -= mouseY;
			_xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

			_cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
		}

		private void Move()
		{
			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");

			Vector3 direction = transform.right * x + transform.forward * z;

			if (direction.magnitude > 1f)
				direction.Normalize();

			float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _moveSpeed;

			Vector3 horizontalVelocity = direction * speed;

			_velocity.y += Physics.gravity.y * Time.deltaTime;

			Vector3 finalVelocity = horizontalVelocity + new Vector3(0f, _velocity.y, 0f);

			_characterController.Move(finalVelocity * Time.deltaTime);
		}

		private void Jump()
		{
			if (_isGrounded && Input.GetButtonDown("Jump"))
				_velocity.y = _jumpForce;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);
		}
		#endregion
	}
}
