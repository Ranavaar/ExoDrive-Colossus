using Deforestation.Machine.Weapon;
using System;
using UnityEngine;

namespace Deforestation.Machine
{
	[RequireComponent (typeof(HealthSystem))]
	public class MachineController : MonoBehaviour
	{
		#region Properties
		public HealthSystem HealthSystem => _health;
		public WeaponController WeaponController;
		public Action<bool> OnMachineDriveChange;
		#endregion

		#region Fields
		private HealthSystem _health;
		private MachineMovement _movement;
		private Animator _anim;
		#endregion

		#region Unity Callbacks
		private void Awake()
		{
			_health = GetComponent<HealthSystem>();
			_movement = GetComponent<MachineMovement>();
			_anim = GetComponent<Animator>();
		}
		void Start()
		{
			_movement.enabled = false;

		}

		void Update()
		{
			
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Water"))
			{
				GameController.Instance.OnWarningPanelOn?.Invoke(true);
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Water"))
			{
				GameController.Instance.OnWarningPanelOn?.Invoke(false);
			}
		}

		#endregion

		#region Public Methods
		public void StopDriving()
		{
			GameController.Instance.MachineMode(false);
			StopMoving();
			OnMachineDriveChange?.Invoke(false);

		}

		public void StartDriving(bool machineMode)
		{
			enabled = machineMode;
			_movement.enabled = machineMode;
			_anim.SetTrigger("WakeUp");
			_anim.SetBool("Move", machineMode);
			OnMachineDriveChange?.Invoke(true);
		}

		public void StopMoving()
		{
			_movement.enabled = false;
			_anim.SetBool("Move", false);
		}
		#endregion

		#region Private Methods
		
		#endregion
	}

}