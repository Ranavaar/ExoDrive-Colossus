using UnityEngine;
using Photon.Realtime;
using System;
using Photon.Pun;

namespace Deforastation.Network
{

	public class NetworkController : MonoBehaviourPunCallbacks
	{
		[SerializeField] private UINetwork _ui;
		private void Start()
		{
			PhotonNetwork.ConnectUsingSettings();
		}
		public override void OnConnectedToMaster()
		{
			PhotonNetwork.JoinOrCreateRoom("ExoDriveRoom", new RoomOptions { MaxPlayers = 10 }, null);
		}
		public override void OnJoinedRoom()
		{
			
			PhotonNetwork.Instantiate("PlayerMultiPlayer", new Vector3(1773, 76.47f, 736), Quaternion.identity);
			PhotonNetwork.Instantiate("TheMachine", new Vector3(1723, 76.47f, 736), Quaternion.identity);
			_ui.LoadingComplete();
		}

	}
}
