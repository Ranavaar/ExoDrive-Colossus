using UnityEngine;
using DG.Tweening;
using System;

namespace Deforestation.Audio
{
	public class AudioController : MonoBehaviour
	{
		const float MAX_VOLUME = 0.1f;

		#region Fields
		[Header("FX")]
		[SerializeField] private AudioSource _steps;
		[SerializeField] private AudioSource _machineOn;
		[SerializeField] private AudioSource _machineOff;
		[SerializeField] private AudioSource _machineStandby;
		[SerializeField] private AudioSource _machineAcceleration;
		[SerializeField] private AudioSource _shoot;

		[Space(10)]

		[Header("Music")]
		[SerializeField] private AudioSource _musicMachine;
		[SerializeField] private AudioSource _musicHuman;
		private bool _isMove;
		private bool _isDrive;
		#endregion

		#region Properties
		#endregion

		#region Unity Callbacks	
		private void Awake()
		{
			GameController.Instance.OnMachineModeChange += SetMachineMusicState;
			GameController.Instance.MachineController.OnMachineDriveChange += SetMachineDriveEffect;
			GameController.Instance.MachineController.WeaponController.OnMachineShoot += ShootFX;
			GameController.Instance.InputSystem.OnMove += Move;
		}

		private void Start()
		{
			_musicHuman.Play();
		}

		private void Update()
		{
			if (_isMove)
			{
				_machineStandby.Stop();
				if (GameController.Instance.MachineModeOn)
				{
					if (!_machineAcceleration.isPlaying)
						_machineAcceleration.Play();
				}
				else
					if (!_steps.isPlaying)
						_steps.Play();
			}
			else
			{
				if (_steps.isPlaying || _machineAcceleration.isPlaying)
				{
					_steps.Stop();
					_machineAcceleration.Stop();
				}
				if (_isDrive)
				{
					if (!_machineOn.isPlaying)
						if (!_machineStandby.isPlaying)
							_machineStandby.Play();

				}

			}

		}
		#endregion

		#region Private Methods
		private void SetMachineMusicState(bool machineMode)
		{
			if (machineMode)
			{
				_musicHuman.DOFade(0, 3);
				_musicMachine.DOFade(MAX_VOLUME, 3);
				_musicMachine.Play();
			}
			else
			{
				_musicHuman.DOFade(MAX_VOLUME, 3);
				_musicMachine.DOFade(0, 3);
			}
		}

		private void SetMachineDriveEffect(bool startDriving)
		{
			if (startDriving)
			{
				_machineOn.Play();
				_isDrive = true;
			}

			else
			{
				_machineOff.Play();
				_isDrive = false;
			}


		}
		private void ShootFX()
		{
			_shoot.Play();
		}
		private void Move()
		{
			_isMove = true;
		}
		#endregion

		#region Public Methods
		#endregion

	}

}