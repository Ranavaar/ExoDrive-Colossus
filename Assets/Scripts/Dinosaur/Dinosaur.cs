using System;
using UnityEngine;
using UnityEngine.AI;

namespace Deforestation.Dinosaurus
{

	public class Dinosaur : MonoBehaviour
	{
		#region Fields
		protected Animator _anim;
		protected NavMeshAgent _agent;
		protected HealthSystem _health;
		protected bool _isDeath;
		protected float _deathTime = 60;
		#endregion

		#region Properties
		public HealthSystem Health => _health;
		#endregion

		#region Unity Callbacks	
		private void Awake()
		{
			_health = GetComponent<HealthSystem>();
			_anim = GetComponent<Animator>();
			_agent = GetComponent<NavMeshAgent>();

			_health.OnDeath += Die;
			_isDeath = false;
		}

		private void Die()
		{
			if (!_isDeath)
			{
				_anim.SetTrigger("Die");
				_isDeath = true;
			}
			if (_deathTime <= 0)
				Destroy(gameObject);
		}
		#endregion

	}

}