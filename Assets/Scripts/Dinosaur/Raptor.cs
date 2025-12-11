using UnityEngine;
using System;
using UnityEngine.AI;
using Deforestation.Machine;

namespace Deforestation.Dinosaurus
{
	public class Raptor : Dinosaur
	{
		#region Fields
		[SerializeField] private float _distanceDetection = 50;
		[SerializeField] private float _distanceEscape = 40;
		[SerializeField] private float _attackDistance = 10;
		private CharacterController _player;
		private MachineController _machine;

		private bool _chase;
		private bool _attack;

		[SerializeField] private float _attackTime = 2;
		[SerializeField] private float _attackDamage = 5;
		private float _attackColdDown;
		#endregion

		#region Unity Callbacks	
		private void Start()
		{
			_player = GameController.Instance != null ? GameController.Instance.CharacterController : null;
			_player = GameController.Instance.CharacterController;
			_machine = GameController.Instance.MachineController;
			_attackColdDown = _attackTime;
		}
		private void Update()
		{
			if(_player == null)
				return;
			if (_machine == null)
				return;
			Vector3 playerPosition = _player.transform.position;
			Vector3 machinePosition= _machine.transform.position;
			if (!_isDeath)
			{
				
				if (GameController.Instance.MachineModeOn)
				{
					if (Vector3.Distance(transform.position, machinePosition) < _distanceDetection)
					{
						Escape();
						Vector3 directionAway = (transform.position - machinePosition).normalized;
						Vector3 fleeTarget = transform.position + directionAway * _distanceEscape;

						NavMeshHit hit;
						if (NavMesh.SamplePosition(fleeTarget, out hit, _attackDistance, NavMesh.AllAreas))
							_agent.SetDestination(hit.position);

					}
				}
				else
				{
					if (Vector3.Distance(transform.position, playerPosition) < _distanceDetection)
						ChasePlayer();
				}


				if (_chase && Vector3.Distance(transform.position, playerPosition) < _attackDistance)
				{
					Attack();
					return;
				}
				if (_chase && Vector3.Distance(transform.position, playerPosition) > _distanceDetection)
				{
					Idle();
					return;
				}

				//Attack
				if (_attack)
				{
					//Atack damage
					_attackColdDown -= Time.deltaTime;
					if (_attackColdDown <= 0)
					{
						_attackColdDown = _attackTime;
						GameController.Instance.PlayerController.HealthSystem.TakeDamage(_attackDamage);
					}
				}
				if (_attack && Vector3.Distance(transform.position, playerPosition) > _attackDistance)
				{
					Chase();
					return;
				}
			}
			else
				_deathTime -= Time.deltaTime;
		}


		#endregion

		#region Private Methods
		private void Idle()
		{
			_anim.SetBool("Run", false);
			_anim.SetBool("Attack", false);
			_chase = false;
			_attack = false;
			_agent.isStopped = true;

		}

		private void Chase()
		{
			if (_player == null)
				return;
			Vector3 playerPosition = _player.transform.position;

			_anim.SetBool("Run", true);
			_anim.SetBool("Attack", false);
			_agent.SetDestination(playerPosition);
			_chase = true;
			_attack = false;
		}

		private void Attack()
		{
			_anim.SetBool("Run", false);
			_anim.SetBool("Attack", true);
			_agent.isStopped = true;
			_chase = false;
			_attack = true;
		}
		private void Escape()
		{
			_anim.SetBool("Run", true);
			_anim.SetBool("Attack", false);
			_chase = false;
			_attack = false;
		}
		private void ChasePlayer()
		{
			if (_player == null)
				return;
			Vector3 playerPosition = _player.transform.position;
			if (!_chase && !_attack && Vector3.Distance(transform.position, playerPosition) < _distanceDetection)
			{
				Chase();
				return;
			}

			//Chase
			if (_chase)
			{
				NavMeshHit hit;
				if (NavMesh.SamplePosition(playerPosition, out hit, _attackDistance, 1))
					_agent.SetDestination(hit.position);
			}
		}
		#endregion


		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _distanceDetection);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _attackDistance);
		}

	}
}
