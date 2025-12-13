using UnityEngine;
using System;
using Photon.Pun;
using Deforestation;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Deforastation.Player;
using Deforastation.Inputs;
using Deforastation.Network;
using Deforestation.Machine;

namespace Deforastation.NetworkPlayer
{

	public class NetworkMachine : MonoBehaviourPun
	{
		#region Properties
		
		#endregion

		#region Fields				
		[SerializeField] private MachineController _machine;	
		[SerializeField] private Transform _machineFollow;	
		private NetworkGameController _gameController;
		#endregion

		#region Unity Callbacks
		// Start is called before the first frame update
		void Start()
		{
			_gameController = FindObjectOfType<NetworkGameController>(true);

			if (photonView.IsMine)
			{
				_gameController.InitializedMachine(_machineFollow, _machine);
				_gameController.gameObject.SetActive(true);
			}
			else
			{

			}
		}

		// Update is called once per frame
		void Update()
		{

		}
		#endregion

		#region Public Methods
		#endregion

		#region Private Methods
		#endregion

	}
}
