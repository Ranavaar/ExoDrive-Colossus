using Deforestation.Machine;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Deforestation.Dinosaurus
{
	public class Pachycephalasaurus : Dinosaur
	{
		#region Fields
		[SerializeField] private float _distanceDetection = 50;
		[SerializeField] private float _attackDistance = 10;
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
			_machine = GameController.Instance != null ? GameController.Instance.MachineController : null;
			_chase = false;
			_attack = false;
			_attackColdDown = _attackTime;
		}
		private void FixedUpdate()
		{
			if (_machine == null)
				return;
			Vector3 machinePosition = _machine.transform.position;

			if (!_isDeath)
			{
				//Idle
				if (!_chase && !_attack && Vector3.Distance(transform.position, machinePosition) < _distanceDetection)
				{
					ChaseMachine();
					return;
				}

				//Chase
				if (_chase)
				{
					NavMeshHit hit;
					if (NavMesh.SamplePosition(machinePosition, out hit, _attackDistance, 1))
						_agent.SetDestination(hit.position);
				}

				if (_chase && Vector3.Distance(transform.position, machinePosition) < _attackDistance)
				{
					Attack();
					return;
				}
				if (_chase && Vector3.Distance(transform.position, machinePosition) > _distanceDetection)
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
						GameController.Instance.MachineController.HealthSystem.TakeDamage(_attackDamage);
					}
				}
				if (_attack && Vector3.Distance(transform.position, machinePosition) > _attackDistance)
				{
					ChaseMachine();
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

		private void ChaseMachine()
		{
			if (_machine == null)
				return;
			Vector3 machinePosition = _machine.transform.position;
			_anim.SetBool("Run", true);
			_anim.SetBool("Attack", false);
			_agent.SetDestination(machinePosition);
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