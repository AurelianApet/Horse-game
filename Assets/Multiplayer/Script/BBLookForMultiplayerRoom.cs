using UnityEngine;
using System.Collections;

#if USE_PHOTON

using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;


public class BBLookForMultiplayerRoom : Photon.MonoBehaviour {

#else
	public class BBLookForMultiplayerRoom {
#endif

#if USE_PHOTON

			

	public PhotonLogLevel _logLevel;
	
	private enum BBRegionList {asia,au,eu,jp,us,none};
	private BBRegionList Current_BBRegion; 
	public int[] pingList;
	public int[] roomsList;
	

	// Use this for initialization
	public IEnumerator ExecuteStart () {
		
		yield return new WaitForSeconds(0.0001f);
		
			Debug.Log("====================== > start < ======================= : " + System.DateTime.Now);
		pingList = new int[5];
		roomsList = new int[5];
		
		PhotonNetwork.logLevel = _logLevel;
		
		PhotonNetwork.automaticallySyncScene = true;
		
		PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.asia);
		PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
		
		Current_BBRegion = BBRegionList.asia;
		
	}
	
	void OnDisconnectedFromPhoton() {
		StartCoroutine(getRoomInRegionAfterDisconnected());
	}
	
	void OnConnectedToPhoton(){
	}
	
	void OnJoinedLobby(){
	
			PhotonNetwork.GetRoomList();
		
	}
	
//	bool waitingForRealRoomsList = false;
	
	IEnumerator OnReceivedRoomListUpdate() { 
		
		
			yield return new WaitForEndOfFrame();
			
			int roomNumber = PhotonNetwork.GetRoomList().Length;
			
			StartCoroutine(getRoomInRegionAfterGotRoomsNumber(roomNumber));
			
	}
	
	
	IEnumerator getRoomInRegionAfterDisconnected() {
		
		
		yield return new WaitForEndOfFrame();
		
		switch(Current_BBRegion) {
		case BBRegionList.asia:
			
			break;
		case BBRegionList.au:
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.au);
			PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			break;
		case BBRegionList.eu:
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.eu);
			PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			break;
		case BBRegionList.jp:
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.jp);
			PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			break;
		case BBRegionList.us:
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.us);
			PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			break;
		case BBRegionList.none:
			gotSearchEnds();
			break;
			
		}
		
		
	}
	
	IEnumerator getRoomInRegionAfterConnected() {
		
		
		
		yield return new WaitForEndOfFrame();
		
		
	}
	
	void gotSearchEnds () {
		
		int openRooms = 0;
		
		foreach(int v in roomsList) {
			openRooms += v;
		}

	
		
		SendMessage("gotRoomsCheckResult",openRooms,SendMessageOptions.DontRequireReceiver);
		
	}
	
	
	IEnumerator getRoomInRegionAfterGotRoomsNumber(int roomNumber) {
		
		int i_ping = PhotonNetwork.GetPing();
		
		
		
		yield return new WaitForEndOfFrame();
		
		switch(Current_BBRegion) {
		case BBRegionList.asia:
			pingList[0] = i_ping;
			roomsList[0] = roomNumber;
			Current_BBRegion = BBRegionList.au;
			PhotonNetwork.Disconnect();
			
			break;
		case BBRegionList.au:
			pingList[1] = i_ping;
			roomsList[1] = roomNumber;
			Current_BBRegion = BBRegionList.eu;
			PhotonNetwork.Disconnect();
			
			break;
		case BBRegionList.eu:
			pingList[2] = i_ping;
			roomsList[2] = roomNumber;
			Current_BBRegion = BBRegionList.jp;
			PhotonNetwork.Disconnect();
			
			break;
		case BBRegionList.jp:
			pingList[3] = i_ping;
			roomsList[3] = roomNumber;
			Current_BBRegion = BBRegionList.us;
			PhotonNetwork.Disconnect();
			
			break;
		case BBRegionList.us:
			pingList[4] = i_ping;
			roomsList[4] = roomNumber;
			Current_BBRegion = BBRegionList.none;
			PhotonNetwork.Disconnect();
			
			break;
			
		}
		
		
		
	}
	
#endif	
	
}
