using UnityEngine;
using System;
using System.Collections;
namespace Deforestation.Interaction
{
	#region Enum
	public enum MachineInteractionType
	{
		Door,
		Stairs,
		Machine
	}
	#endregion

	public class MachineInteraction : MonoBehaviour, IInteractable
	{
		#region Fields
		[SerializeField] protected MachineInteractionType _type;
		[SerializeField] protected Transform _target;
		[SerializeField] protected Transform _initialPos;
		[SerializeField] protected InteractableInfo _interactableInfo;
		#endregion

		#region Unity Callbacks
		private void Start()
		{
			if (_initialPos == null)
				return;
		}
		#endregion

		#region Public Methods
		public InteractableInfo GetInfo()
		{
			_interactableInfo.Type = _type.ToString();
			return _interactableInfo;
		}

		public virtual void Interact()
		{
			if (_type == MachineInteractionType.Door)
			{
				StartCoroutine(DoorRoutine());
			}
			if (_type == MachineInteractionType.Stairs)
			{
				GameController.Instance.TeleportPlayer(_target.position);
			}
			if (_type == MachineInteractionType.Machine)
			{
				GameController.Instance.MachineMode(true);
			}
		}

		#endregion

		private IEnumerator DoorRoutine()
		{
			transform.position = _target.position;

			yield return new WaitForSeconds(5);

			transform.position = _initialPos.position;
		}
	}

}