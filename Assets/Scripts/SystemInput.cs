using UnityEngine;
using System;
using Deforestation;

namespace Deforastation.Inputs
{
	public class SystemInput : MonoBehaviour
	{
		public Action OnMove;
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

			if (Input.GetAxis("Horizontal") != 0 && GameController.Instance.MachineModeOn || Input.GetAxis("Vertical") != 0 && GameController.Instance.MachineModeOn)
				OnMove?.Invoke();
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				if (GameController.Instance.MachineModeOn)
					GameController.Instance.MachineController.StopDriving();
				else
					GameController.Instance.UIController.MenuOn();
			}

		}
		#endregion

	}
}
