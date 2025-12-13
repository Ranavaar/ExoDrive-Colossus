using UnityEngine;
using System;
using Deforestation.UI;

namespace Deforastation.Network
{

	public class UINetwork : MonoBehaviour
	{
		#region Properties
		#endregion

		#region Fields
		[SerializeField] private GameObject _conectingPanel;
		[SerializeField] private UIGameController _uiGameController;

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
		public void LoadingComplete()
		{
			_conectingPanel.SetActive(false);
			_uiGameController.enabled = true;
		}
		#endregion

		#region Private Methods
		#endregion

	}
}
