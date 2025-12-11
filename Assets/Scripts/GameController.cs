using UnityEngine;
using UnityEngine.SceneManagement;
using Deforestation.Machine;
using Deforestation.UI;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Cinemachine;
using System;
using Deforastation.Player;
using Deforastation.Inputs;


namespace Deforestation
{
	public class GameController : Singleton<GameController>
	{
		#region Properties
		public MachineController MachineController => _machine;
		public CharacterController CharacterController => _characterController;
		public PlayerController PlayerController => _playerController;
		public Inventory Inventory => _inventory;
		public InteractionSystem InteractionSystem => _interactionSystem;
		public TreeTerrainController TerrainController => _terrainController;
		public Camera MainCamera;
		public SystemInput InputSystem => _inputSystem;
		public UIGameController UIController => _uiController;

		//Events
		public Action<bool> OnMachineModeChange;
		public Action<bool> OnWarningPanelOn;

		public bool MachineModeOn
		{
			get
			{
				return _machineModeOn;
			}
			private set
			{
				_machineModeOn = value;
				OnMachineModeChange?.Invoke(_machineModeOn);
			}
		}
		#endregion

		#region Fields
		[Header("Player")]
		[SerializeField] protected CharacterController _characterController;
		[SerializeField] protected PlayerController _playerController;
		[SerializeField] protected HealthSystem _playerHealth;
		[SerializeField] protected Inventory _inventory;
		[SerializeField] protected InteractionSystem _interactionSystem;
		[Header("Input")]
		[SerializeField] protected SystemInput _inputSystem; 

		[Header("Camera")]
		[SerializeField] protected CinemachineVirtualCamera _virtualCamera;
		[SerializeField] protected Transform _playerFollow;
		[SerializeField] protected Transform _machineFollow;

		[Header("Machine")]
		[SerializeField] protected MachineController _machine;
		[Header("UI")]
		[SerializeField] protected UIGameController _uiController;
		[Header("Trees Terrain")]
		[SerializeField] protected TreeTerrainController _terrainController;

		private bool _machineModeOn;
		#endregion

		#region Unity Callbacks
		void Start()
		{
			//UI Update
			_playerHealth.OnHealthChanged += _uiController.UpdatePlayerHealth;
			_playerHealth.OnDeath += Death;
			_machine.HealthSystem.OnHealthChanged += _uiController.UpdateMachineHealth;
			_machine.HealthSystem.OnDeath += Death;
			MachineModeOn = false;
		}

		void Update()
		{
			if (_machine != null)
			{
				if (_machine.transform.position.y < 46)
				{
					Death();
					return;
				}
			}

			if (_characterController != null)
			{
				if (_characterController.transform.position.y < 49)
				{
					Death();
					return;
				}
			}
		}
		#endregion

		#region Public Methods
		public void TeleportPlayer(Vector3 target)
		{
			_characterController.enabled = false;
			_characterController.transform.position = target;
			_characterController.enabled = true;
		}

		internal void MachineMode(bool machineMode)
		{
			MachineModeOn = machineMode;
			//Player
			_characterController.gameObject.SetActive(!machineMode);
			_characterController.enabled = !machineMode;

			//Cursor + UI
			if (machineMode)
			{
				//Start Driving
				if (Inventory.HasResource(RecolectableType.DrivingCrystal))
					_machine.StartDriving(machineMode);

				_characterController.transform.parent = _machineFollow;
				_uiController.HideInteraction();
				Cursor.lockState = CursorLockMode.None;
				//Camera
				_virtualCamera.Follow = _machineFollow;

				_machine.enabled = true;
				_machine.WeaponController.enabled = true;
				_machine.GetComponent<MachineMovement>().enabled = true;

			}
			else
			{
				_machine.enabled = false;
				_machine.WeaponController.enabled = false;
				_machine.GetComponent<MachineMovement>().enabled = false;
				_characterController.transform.parent = null;

				//Camera
				_virtualCamera.Follow = _playerFollow;
				Cursor.lockState = CursorLockMode.Locked;
			}
			Cursor.visible = machineMode;
		}
		#endregion

		#region Private Methods
		private void Death()
		{			
			SceneManager.LoadScene("DeathScene");
		}
		#endregion
	}

}