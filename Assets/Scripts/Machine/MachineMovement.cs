using Deforestation.Dinosaurus;
using Deforestation.Recolectables;
using UnityEngine;

namespace Deforestation.Machine
{
	public class MachineMovement : MonoBehaviour
	{
		#region Fields
		[SerializeField] private float _speedForce = 50;
		[SerializeField] private float _speedRotation = 15;
		[SerializeField] private float _gravityForce = 2.5f;
		[SerializeField] private float _jump;
		[SerializeField] private Transform _checkGround;
		private float _rotate;
		private bool _isGrounded;
		private bool _isMove;
		private Rigidbody _rb;
		private Vector3 _movementDirection;
		private Inventory _inventory => GameController.Instance.Inventory;

		[Header("Energy")]
		[SerializeField] private float energyDecayRate = 20f;
		private float energyTimer = 0;
		#endregion

		#region Unity Callbacks	
		private void Awake()
		{
			_isMove = false;
			_rb = GetComponent<Rigidbody>();
			GameController.Instance.InputSystem.OnMove += Move;
		}

		private void Update()
		{
			if (_inventory.HasResource(RecolectableType.DrivingCrystal))
			{
				//Movement
				_movementDirection = transform.right * Input.GetAxis("Vertical");
				_rotate = Input.GetAxis("Horizontal");
				Debug.DrawRay(transform.position, transform.InverseTransformDirection(_movementDirection.normalized) * _speedForce);

				//Energy
				if (_isMove)
				{
					energyTimer += Time.deltaTime;
					if (energyTimer >= energyDecayRate)
					{
						_inventory.UseResource(RecolectableType.DrivingCrystal);
						energyTimer = 0;
					}
				}
			}
			else
			{
				GameController.Instance.MachineController.StopMoving();
			}
			_isGrounded = Physics.Raycast(_checkGround.position, -transform.up, 20);
			Debug.DrawRay(_checkGround.position, -transform.up * 20, _isGrounded ? Color.green : Color.red);

		}

		private void FixedUpdate()
		{
			if (_isGrounded)
				_rb.AddForce(_movementDirection * _speedForce, ForceMode.Impulse);
			if (Mathf.Abs(_rotate) > 0.001f)
			{
				Quaternion delta = Quaternion.Euler(0f, _rotate * _speedRotation * Time.fixedDeltaTime, 0f);
				_rb.MoveRotation(_rb.rotation * delta);
			}
			if (Input.GetKeyUp(KeyCode.Space) && _isGrounded && _inventory.HasResource(RecolectableType.JumpingCrystal))
				_rb.AddForce(transform.up * _jump, ForceMode.VelocityChange);
			if(!_isGrounded)
				_rb.AddForce(Physics.gravity * _gravityForce, ForceMode.Acceleration);

			if (transform.rotation.z > 70)
				_rb.AddTorque(transform.forward * -1 * Time.deltaTime, ForceMode.Force);
			if (transform.rotation.z < -70)
				_rb.AddTorque(transform.forward * 1 * Time.deltaTime, ForceMode.Force);
			if (transform.rotation.x > 70)
				_rb.AddTorque(transform.right * -1 * Time.deltaTime, ForceMode.Force);
			if (transform.rotation.x < -70)
				_rb.AddTorque(transform.right * 1 * Time.deltaTime, ForceMode.Force);
		}



		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Tree")
			{
				Tree tree = other.GetComponent<Tree>();
				if (tree == null || tree.Index < 0)
					return;

				int index = tree.Index;
				tree.Index = -1;

				if (other.TryGetComponent<Collider>(out Collider collider))
					collider.enabled = false;

				GameController.Instance.TerrainController.DestroyTree(index, other.transform.position);
			}


		}
		private void OnCollisionEnter(Collision collision)
		{
			//Hacemos daño por contacto a los Stegasaurus
			HealthSystem target = collision.gameObject.GetComponent<HealthSystem>();
			Rigidbody noPlayer = collision.gameObject.GetComponent<Rigidbody>();
			if (target != null && noPlayer != null)
			{
				target.TakeDamage(10);
			}
		}

		#endregion

		#region Private Methods
		private void Move()
		{
			_isMove = true;
		}
		#endregion

	}

}
