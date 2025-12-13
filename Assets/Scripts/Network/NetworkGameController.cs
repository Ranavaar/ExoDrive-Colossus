using UnityEngine;
using System;
using Deforestation;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Deforestation.Machine;

namespace Deforastation.Network
{

	public class NetworkGameController : GameController
	{
		#region Properties
		#endregion

		#region Fields
		#endregion

		#region Unity Callbacks
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
		#endregion

		#region Public Methods
		public void InitializedPlayer(HealthSystem health, CharacterController characterController, Inventory inventory, InteractionSystem interactionSystem, Transform playerFollow)
		{
			_playerHealth = health;
			_characterController = characterController;
			_inventory = inventory;
			_interactionSystem = interactionSystem;
			_playerFollow = playerFollow;
		}
		public void InitializedMachine( Transform machineFollow, MachineController machine)
		{
			_machineFollow = machineFollow;
			_machine = machine;
		}
		#endregion

		#region Private Methods
		#endregion

	}
}
