using UnityEngine;
using System;
using Photon.Pun;
using Deforestation;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Deforastation.Player;
using Deforastation.Inputs;
using Deforastation.Network;

namespace Deforastation.NetworkPlayer
{

	public class NetworkPlayer : MonoBehaviourPun
	{
		#region Properties
		#endregion

		#region Fields
		[SerializeField] private HealthSystem _healthSystem;
		[SerializeField] private Inventory _inventory;		
		[SerializeField] private InteractionSystem _interactionSystem;		
		[SerializeField] private CharacterController _characterController;		
		[SerializeField] private PlayerController _playerController;
		//[SerializeField] private SystemInput _inputSystem;		
		[SerializeField] private GameObject _3dAvatar;	
		[SerializeField] private Transform _playerFollow;	
		private Animator _anim;	
		private NetworkGameController _gameController;
		#endregion

		#region Unity Callbacks
		private void Awake()
		{
			_anim = _3dAvatar.GetComponent<Animator>();
		}
		void Start()
		{
			_gameController = FindObjectOfType<NetworkGameController>(true);

			if (photonView.IsMine)
			{
				_gameController.InitializedPlayer(_healthSystem, _characterController, _inventory, _interactionSystem, _playerFollow);
				_healthSystem.OnHealthChanged += Hit;
				_healthSystem.OnDeath += Die;
			}
			else
			{
				DisconetPlayer();
			}
		}


		// Update is called once per frame
		void Update()
		{
			if (photonView.IsMine)
			{
				if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
				{
					_anim.SetBool("Run", true);
				}
				else
					_anim.SetBool("Run", false);
				if (Input.GetKeyUp(KeyCode.Space))
					_anim.SetTrigger("Jump");
			}
		}
		#endregion

		#region Public Methods
		#endregion

		#region Private Methods
		private void Die()
		{
			_anim.SetTrigger("Die");
			DisconetPlayer();
			this.enabled = false;
		}

		private void Hit(float obj)
		{
			_anim.SetTrigger("Hit");
		}
		private void DisconetPlayer()
		{
			Destroy(_healthSystem);
			Destroy(_inventory);
			Destroy(_interactionSystem);
			Destroy(_characterController);
			Destroy(_playerController);
		}
		#endregion

	}
}
