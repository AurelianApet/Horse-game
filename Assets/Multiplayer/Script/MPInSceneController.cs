using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if USE_PHOTON

public class MPInSceneController : Photon.MonoBehaviour {


	public string playerPrefabName = "Player";
	public Transform spawnPoint;

	// Use this for initialization
	void Start () {

 
		Debug.Log("[MPInSceneController][Start] Network : " + PhotonNetwork.connectedAndReady 
		                                                    + " : " + PhotonNetwork.room.name
		                                                    + " : " + PhotonNetwork.player.name
		                                                    + " : " + PhotonNetwork.room.maxPlayers
		                                                    + " : " + PhotonNetwork.room.playerCount

		          );


		GameObject.Find("LabelRoomName").GetComponent<Text>().text =  "Room Name : " + PhotonNetwork.room.name;
		GameObject.Find("LabelRoomSceneName").GetComponent<Text>().text = "Scene Name : " + SceneManager.GetActiveScene().name;
		GameObject.Find("LabelPlayerName").GetComponent<Text>().text = "Player Name : " + (string)PhotonNetwork.player.name;
		GameObject.Find("LabelMaxPlayers").GetComponent<Text>().text = "Room Max Players : " + (string)PhotonNetwork.room.maxPlayers.ToString();
		GameObject.Find("LabelConnected").GetComponent<Text>().text = "Connected : " + (string)PhotonNetwork.connectedAndReady.ToString();

		// here you are inside the room then can start instantiate your player object etc...


		GameObject _Player = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, Quaternion.identity, 0);
		_Player.name = PhotonNetwork.player.name;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void gotMainMenuButton(GameObject _go) {
		Debug.Log("****** gotMainMenuButton ************ " + _go.name);
	 
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.Disconnect();
	}

	void OnDisconnectedFromPhoton() {
		SceneManager.LoadScene(0);
	}

}
#endif
