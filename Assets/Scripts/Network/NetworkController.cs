using UnityEngine;
using Photon.Realtime;
using System;
using Photon.Pun;
using System.Collections.Generic;

namespace Deforastation.Network
{

	public class NetworkController : MonoBehaviourPunCallbacks
	{
		[SerializeField] private UINetwork _ui;

		[SerializeField] private List <Transform> _spawnPoints;
		private bool _waitingForSpawn = false;
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
			if (PhotonNetwork.IsMasterClient)
			{
				SpawnMe(_spawnPoints[0].position);
				_spawnPoints.RemoveAt(0);
			}
			else
			{
				_waitingForSpawn = true;
				photonView.RPC("RPC_SpawnPoint", RpcTarget.MasterClient);
			}

				_ui.LoadingComplete();
		}

		private void SpawnMe(Vector3 spawnPoints)
		{
			PhotonNetwork.Instantiate("PlayerMultiPlayer", spawnPoints, Quaternion.identity);
			PhotonNetwork.Instantiate("TheMachine", spawnPoints, Quaternion.identity);
		}
		[PunRPC]
		void RPC_SpawnPoint()
		{
			photonView.RPC("RPC_RecivePoint", RpcTarget.Others, _spawnPoints[0].position);
			_spawnPoints.RemoveAt(0);
		}
		[PunRPC]
		void RPC_RecivePoint(Vector3 spawnPos)
		{
			if (_waitingForSpawn)
				SpawnMe(spawnPos);
		}
	}
}
