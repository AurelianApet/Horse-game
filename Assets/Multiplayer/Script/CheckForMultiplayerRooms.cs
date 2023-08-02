using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if USE_PHOTON

using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class CheckForMultiplayerRooms : Photon.MonoBehaviour {

  
   public int maxPingToPlay = 500; 
  
   public bool directConnect = false;

	public PhotonLogLevel _logLevel;
		
	private enum BBRegionList {asia,au,eu,jp,us,none};
   private BBRegionList Current_BBRegion; 
   public int[] pingList;
   public int[] roomsList;
   
   public bool canClick = false;
	
	public GameObject baseMainMenuContainer;
	public GameObject CompleteCreateRoomUGUIMultiplayer;
	public GameObject ChooseMapUI;
	public GameObject ContainerChooseMPServer;
	public GameObject MultiplayerRoomsControllerSCRIPT;
	
	InputField InputFieldPlayername;
	InputField InputFieldRoomName;
	Text UILabelInfoMessageOnMPConnecting;
	
	private bool creatingRoomState = false;
	
	private int maxPlayerNumber = 2;
	
	void Awake() {
	    PhotonNetwork.PhotonServerSettings.JoinLobby = true;
	}
	
	
	void gotItemRoomClick(GameObject _go) {
	
		UILabelInfoMessageOnMPConnecting.text = "Loading game...";
		
		BBStaticVariableMultiplayer.currentMPPlayerName = setPrefix() + PlayerPrefs.GetString("PlayerNickName"); //InputFieldPlayername.text;
		PhotonNetwork.playerName = BBStaticVariableMultiplayer.currentMPPlayerName;
		string mapToJoin = _go.transform.FindChild("mapNameToJoin").GetComponent<Text>().text;
		
		BBStaticVariable.BBLog("connected : " + PhotonNetwork.connectedAndReady + " insideLobby : " + PhotonNetwork.insideLobby + " map : " + mapToJoin + " absoluteMaxMoneyWon : " + PlayerPrefs.GetFloat("absoluteMaxMoneyWon"));
		
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"pcountry", PlayerPrefs.GetString("countryCode") }});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"avatarCode", PlayerPrefs.GetInt("MyAvatarCode") }});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"playerCash", PlayerPrefs.GetFloat("absoluteMaxMoneyWon") }});
		
		
		PhotonNetwork.JoinRoom(mapToJoin);
		
	}
	
	
	
	
	public void gotMaxPlayerRoomSelectionUGUI (Toggle _selected) {
		
		if(_selected.name.Contains("Player_2")) maxPlayerNumber = 2;
		else if(_selected.name.Contains("Player_4")) maxPlayerNumber = 4;
		else if(_selected.name.Contains("Player_5")) maxPlayerNumber = 5;
		else if(_selected.name.Contains("Player_8")) maxPlayerNumber = 8;
		
	}
	
	public void gotBackMainMenuButton() {
	   PhotonNetwork.Disconnect();
		ContainerChooseMPServer.SetActive(false);
		CompleteCreateRoomUGUIMultiplayer.SetActive(false);
		baseMainMenuContainer.SetActive(true);
		MultiplayerRoomsControllerSCRIPT.SetActive(false);
		
	}
	
	public void gotCreateRoom() {
	
	   creatingRoomState = true;

		BBStaticVariable.BBLog("[CheckForMultiplayerRooms] gotCreateRoom");
		
		
		BBStaticVariableMultiplayer.currentMPmaxPlayerNumber = maxPlayerNumber;
		BBStaticVariableMultiplayer.currentMPRoomName = setPrefix() + InputFieldRoomName.text;
		BBStaticVariableMultiplayer.currentMPPlayerName = setPrefix() + PlayerPrefs.GetString("PlayerNickName"); //InputFieldPlayername.text;
		
		BBStaticVariable.BBLog("***************BBStaticVariable.currentMPmaxPlayerNumber : " + BBStaticVariableMultiplayer.currentMPmaxPlayerNumber);
		BBStaticVariable.BBLog("***************BBStaticVariable.currentMPRoomName : " + BBStaticVariableMultiplayer.currentMPRoomName);
		BBStaticVariable.BBLog("***************BBStaticVariable.currentMPPlayerName : " + BBStaticVariableMultiplayer.currentMPPlayerName);
		
		CompleteCreateRoomUGUIMultiplayer.SetActive(false);
		ChooseMapUI.SetActive(true);
		MultiplayerRoomsControllerSCRIPT.SetActive(true);
		
		
	}
	
	
	public void gotConnectButton(GameObject _go) {
		
		if( GameObject.Find("CheckForMultiplayerRooms").GetComponent<CheckForMultiplayerRooms>().canClick ) {
			
			switch(_go.name) {
				
			case "ButtonASIA":
				BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.asia;
				break;
			case "ButtonAU":
				BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.au;
				break;
			case "ButtonEU":
				BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.eu;
				break;
			case "ButtonJAPAN":
				BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.jp;
				break;
			case "ButtonUSA":
				BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.us;
				break;
			case "ButtonGOTO_MENU":
				break;
				
			}
			
		}
		
	    ContainerChooseMPServer.SetActive(false);
	    CompleteCreateRoomUGUIMultiplayer.SetActive(true);
	
		if(!InputFieldRoomName) InputFieldRoomName = GameObject.Find("InputFieldRoomName").GetComponent<InputField>();
		if(!UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting = GameObject.Find("UILabelInfoMessageOnMPConnecting").GetComponent<Text>();
		
		int r = UnityEngine.Random.Range(1,999);
		InputFieldRoomName.text = "Room_" + r.ToString();
		
		UILabelInfoMessageOnMPConnecting.text = "Connecting to server...: " + BBStaticVariableMultiplayer.selectedRegionCode ;
		
		waitingForRealRoomsList = true;
		
		if(!PhotonNetwork.connectedAndReady) {
 			PhotonNetwork.ConnectToRegion(BBStaticVariableMultiplayer.selectedRegionCode,BBStaticVariableMultiplayer.photonConnectionVersion);
		}
	
	}

		
	void OnDisable () {
	 
		if(!waitingForRealRoomsList) {
		
		      if(PhotonNetwork.connectedAndReady) {
			     PhotonNetwork.Disconnect();
			   }
			   this.enabled = false;
	    }
	  
	}
	
	// Use this for initialization
	void OnEnable() {
				
		setAllButtoDisabled();
		
		BBStaticVariable.BBLog("====================== > start < ======================= : " + System.DateTime.Now + " isConnected : " + PhotonNetwork.connected);
		
	   pingList = new int[5];
	   roomsList = new int[5];
	
	    PhotonNetwork.logLevel = _logLevel;
		
	    PhotonNetwork.automaticallySyncScene = true;
		
	   if(!directConnect) {	
		    PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.asia);
			Current_BBRegion = BBRegionList.asia;
			
			if(!PhotonNetwork.connected) {
			    PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
	         } else {
	           PhotonNetwork.Disconnect();
	         }
		    
	   } else {
	   
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.eu);
			
			if(!PhotonNetwork.connected) {
				PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			} else {
				PhotonNetwork.Disconnect();
			}
			
			Current_BBRegion = BBRegionList.eu;
		}
	
	}
	
	// Update is called once per frame
	void gotSearchEnds () {
	
		BBStaticVariable.BBLog("====================== > gotSearchEnds < ======================= : " + System.DateTime.Now);
		GameObject.Find("LabelLOADING").GetComponent<Text>().text = "Done....";
		
		canClick = true;
	}
	
	IEnumerator getRoomInRegionAfterDisconnected() {
	
		BBStaticVariable.BBLog("====================== > getRoomInRegionAfterDisconnected : " + Current_BBRegion);
		
		yield return new WaitForEndOfFrame();
		
		switch(Current_BBRegion) {
		case BBRegionList.asia:
			PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
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
		
		BBStaticVariable.BBLog("====================== > getRoomInRegionAfterConnected : " + Current_BBRegion);
		
		yield return new WaitForEndOfFrame();
		
		
	}
	
				
	IEnumerator getRoomInRegionAfterGotRoomsNumber(int roomNumber) {
	    
		int i_ping = PhotonNetwork.GetPing();
	    
		BBStaticVariable.BBLog("====================== > getRoomInRegion : " + Current_BBRegion + " number : " + roomNumber + " ping : " + i_ping);
	
	   yield return new WaitForEndOfFrame();
	   
	   switch(Current_BBRegion) {
	       case BBRegionList.asia:
	          pingList[0] = i_ping;
	          roomsList[0] = roomNumber;
			  setResultData("LabelASIA_PING","LabelASIA_ROOMS","ButtonASIA",pingList[0],roomsList[0]);
	          Current_BBRegion = BBRegionList.au;
	          PhotonNetwork.Disconnect();
	       
	       break;
		   case BBRegionList.au:
			  pingList[1] = i_ping;
			  roomsList[1] = roomNumber;
			  setResultData("LabelAU_PING","LabelAU_ROOMS","ButtonAU",pingList[1],roomsList[1]);
			  Current_BBRegion = BBRegionList.eu;
			  PhotonNetwork.Disconnect();
			
			break;
			case BBRegionList.eu:
				pingList[2] = i_ping;
			    roomsList[2] = roomNumber;
			    setResultData("LabelEU_PING","LabelEU_ROOMS","ButtonEU",pingList[2],roomsList[2]);
			    Current_BBRegion = BBRegionList.jp;
				PhotonNetwork.Disconnect();
				
				break;
			case BBRegionList.jp:
				pingList[3] = i_ping;
			    roomsList[3] = roomNumber;
			    setResultData("LabelJP_PING","LabelJP_ROOMS","ButtonJAPAN",pingList[3],roomsList[3]);
				Current_BBRegion = BBRegionList.us;
				PhotonNetwork.Disconnect();
				
				break;
			case BBRegionList.us:
				pingList[4] = i_ping;
			    roomsList[4] = roomNumber;
			    setResultData("LabelUSA_PING","LabelUSA_ROOMS","ButtonUSA",pingList[4],roomsList[4]);
				Current_BBRegion = BBRegionList.none;
				PhotonNetwork.Disconnect();
				
				break;
			
	   }
	   
	   
	   	
	}
	
	void goToMainMenu () {
	
	  SceneManager.LoadScene(0);
	  
	}
	
	void OnFailedToConnectToPhoton(DisconnectCause cause) { 
		
		print("Failed To Connect To Server : " + cause.ToString());
		
		GameObject.Find("LabelLOADING").GetComponent<Text>().text = "NO CONNECTION TO SERVER TRY AGAING LATER....";
		canClick = true;
		
		Invoke("goToMainMenu",8);

	}
	
	void OnDisconnectedFromPhoton() {
		print ("OnDisconnectedFromPhoton");
		 
		if(waitingForRealRoomsList) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			// waitingForRealRoomsList = false;
		    // gameObject.SetActive(false);
		} else {
		   StartCoroutine(getRoomInRegionAfterDisconnected());
		}
	}
	
	void OnConnectedToPhoton(){
		print ("OnConnectedToPhoton : ");
		if(UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting.text = "Connected";
	}

	void OnJoinedLobby(){
		print ("OnJoinedLobby creatingRoomState : " + creatingRoomState);
		
		if(creatingRoomState) {
			
		} else {
		     PhotonNetwork.GetRoomList();
		}
		
		
	}

	IEnumerator OnPhotonCreateRoomFailed() { 
		print ("Failed OnPhotonCreateRoomFailed");
		UILabelInfoMessageOnMPConnecting.text = "On Create Room Failed!!!";
		
		yield return new WaitForSeconds(5);
		
		waitingForRealRoomsList = true;
		PhotonNetwork.Disconnect();
		
	}
	
	IEnumerator OnPhotonJoinRoomFailed(){
		print ("Failed on connecting to room");
		UILabelInfoMessageOnMPConnecting.text = "On Join Room Failed";
		
		yield return new WaitForSeconds(5);
		
		waitingForRealRoomsList = true;
		PhotonNetwork.Disconnect();
	}
	
			
	bool waitingForRealRoomsList = false;
	
	IEnumerator OnReceivedRoomListUpdate() { 
	
		print ("OnReceivedRoomListUpdate beginning waitingForRealRoomsList : " + waitingForRealRoomsList); 
	  
	  	if(!directConnect) {	
			if(waitingForRealRoomsList) {
			    StartCoroutine( setJoinRoomButtons() );
			} else {
					yield return new WaitForEndOfFrame();
					
					int roomNumber = PhotonNetwork.GetRoomList().Length;
					
					print ("OnReceivedRoomListUpdate : " + roomNumber);
					
					
					foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList()) {
						
						BBStaticVariable.BBLog("RName -> " + roomInfo.name);
						
					}
					
					StartCoroutine(getRoomInRegionAfterGotRoomsNumber(roomNumber));
					
			}
		} else {
			BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.eu;
			ContainerChooseMPServer.SetActive(false);
			CompleteCreateRoomUGUIMultiplayer.SetActive(true);
			
			//if(!InputFieldPlayername) InputFieldPlayername = GameObject.Find("InputFieldPlayername").GetComponent<InputField>();
			if(!InputFieldRoomName) InputFieldRoomName = GameObject.Find("InputFieldRoomName").GetComponent<InputField>();
			if(!UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting = GameObject.Find("UILabelInfoMessageOnMPConnecting").GetComponent<Text>();
			
			int r = UnityEngine.Random.Range(1,999);
			//InputFieldPlayername.text = "Player_" + r.ToString();
			InputFieldRoomName.text = "Room_" + r.ToString();
			
			UILabelInfoMessageOnMPConnecting.text = "Connecting to server...";
			
			waitingForRealRoomsList = true;
			
			if(!PhotonNetwork.connectedAndReady) {
				PhotonNetwork.OverrideBestCloudServer(BBStaticVariableMultiplayer.selectedRegionCode);
				PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			}
			
			
		}
	}
	
	Color getPingColor(int val) {
	    BBStaticVariable.BBLog("color : " + val + "  _  " + Current_BBRegion);
		
		if(val >= 500) return Color.red;
		else if( (val <= 449) && (val >= 150) ) return Color.yellow;
		else if( val <= 149 ) return Color.green;
		else return Color.gray;
		
	}
	
	void setAllButtoDisabled() {
	
					   StartCoroutine( setButtonDisabled("ButtonASIA") );
					   StartCoroutine( setButtonDisabled("ButtonAU") );
		               StartCoroutine( setButtonDisabled("ButtonEU") );
		               StartCoroutine( setButtonDisabled("ButtonJAPAN") );
		               StartCoroutine( setButtonDisabled("ButtonUSA") );
		
		
		
	}
	
	IEnumerator setButtonDisabled(string lab) {
	    yield return new WaitForEndOfFrame();
		GameObject g = GameObject.Find(lab);
		
		g.GetComponent<Button>().enabled = false;
		g.transform.FindChild("Label").GetComponent<Text>().color = Color.grey;
	}
	
	
	void setResultData(string pingLab, string roomsLab, string buttLab, int pingValue, int roomsValue) {
	
		GameObject.Find(pingLab).GetComponent<Text>().text = "Ping : " + pingValue;
		GameObject.Find(pingLab).GetComponent<Text>().color = getPingColor(pingValue);
		if(getPingColor(pingValue) == Color.red) {
			GameObject g = GameObject.Find(buttLab);
			g.GetComponent<Button>().enabled = false;
			g.transform.FindChild("Label").GetComponent<Text>().color = Color.grey;
		} else {
			GameObject g = GameObject.Find(buttLab);
			g.GetComponent<Button>().enabled = true;
			g.transform.FindChild("Label").GetComponent<Text>().color = Color.green;
		}
		GameObject.Find(roomsLab).GetComponent<Text>().text = "Games : " + roomsValue;
	
	}
	
		public bool isTestingForRoomsbutt = false;
	
	IEnumerator setJoinRoomButtons() {
	yield return new WaitForEndOfFrame();

		BBStaticVariable.BBLog("***********setJoinRoomButtons***************");
		
		GameObject[] _items = GameObject.FindGameObjectsWithTag("roomButton");		
		foreach (GameObject itGO in _items) { Destroy(itGO);}
		yield return new WaitForEndOfFrame();
		

		
		GameObject YESRoomsItem = Resources.Load("MultiplayerItemRoomUUI") as GameObject;
		GameObject NORoomsItem = Resources.Load("MultiplayerItemRoomNoRoomsUUI") as GameObject;
		
		GameObject PanelScrollRoot = GameObject.Find("PanelBUTTONS_Rooms");
		
		BBStaticVariable.BBLog("***********setJoinRoomButtons*************** " + PanelScrollRoot.name);
		
		
		if(isTestingForRoomsbutt) {
			
			for(int i = 0; i < 20; i++) {
				GameObject inst = (GameObject)Instantiate(YESRoomsItem);
				
				inst.transform.SetParent(PanelScrollRoot.transform, false);
				
				inst.transform.FindChild("UILabelPlayersNum").GetComponent<Text>().text = "6 / 6";
				inst.transform.FindChild("UILabelRoomName").GetComponent<Text>().text = "text Room ciao";
				inst.transform.FindChild("UILabelButtJoin").GetComponent<Text>().text = i.ToString();
				
			}
			
		} else {
			
			int _rNum = PhotonNetwork.GetRoomList().Length;
			BBStaticVariable.BBLog("------------->> numOfRooms : " + _rNum);
			//UILabelInfoMessageOnMPConnecting.text = "Opened Games : " + _rNum;
			if(_rNum > 0) {			
				foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList()) {
					
					BBStaticVariable.BBLog("RName -> " + roomInfo.name);
					
					GameObject goTMP = GameObject.Find(roomInfo.name); 
					
					if(goTMP == null) {
						GameObject inst = (GameObject)Instantiate(YESRoomsItem);
						inst.transform.SetParent(PanelScrollRoot.transform, false);
						//inst.name = roomInfo.name;
						
						inst.transform.FindChild("UILabelPlayersNum").GetComponent<Text>().text = roomInfo.playerCount + "/" + roomInfo.maxPlayers;
						string thirdElement = roomInfo.name.Substring(2,1);
						string mapID = "0";
						  if(thirdElement == "]") { //mapID 0..9
						    mapID = roomInfo.name.Substring(1,1);
							inst.name = roomInfo.name.Substring(3);
							inst.transform.FindChild("UILabelRoomName").GetComponent<Text>().text = roomInfo.name.Substring(3);
						  } else { //mapID 10....->
							mapID = roomInfo.name.Substring(1,2);
							inst.name = roomInfo.name.Substring(4);
							inst.transform.FindChild("UILabelRoomName").GetComponent<Text>().text = roomInfo.name.Substring(4);
						  }
						  int i_mapID = int.Parse(mapID);
						  BBStaticVariable.BBLog("thirdElement : " + thirdElement + " mapID : " + mapID);
						  inst.transform.FindChild("RawImage").GetComponent<RawImage>().texture =  MultiplayerRoomsControllerSCRIPT.GetComponent<MultiplayerRoomsController>().allMaps[i_mapID].mapPreview;
						  inst.transform.FindChild("mapNameToJoin").GetComponent<Text>().text = roomInfo.name;
					}			
					
				}
			} else {
				
				GameObject goTMP = GameObject.Find("noRoomsItem"); 
				
				if(goTMP == null) {
					GameObject inst = (GameObject)Instantiate(NORoomsItem);
					inst.transform.SetParent(PanelScrollRoot.transform, false);
					inst.name = "noRoomsItem";
				}
			}
		}
		
		
	}
	
		
	
	string setPrefix() {
		if(Application.platform == RuntimePlatform.Android) return "[AND]";
		else if(Application.platform == RuntimePlatform.IPhonePlayer) return "[IOS]";
		else if(Application.platform == RuntimePlatform.WindowsEditor) return "[DEV]";
		else if(Application.platform == RuntimePlatform.BlackBerryPlayer ) return "[BB]";
		else if(Application.platform == RuntimePlatform.OSXPlayer) return "[MAC]";
		else if(Application.platform == RuntimePlatform.WSAPlayerARM) return "[WIN8]";
		else if(Application.platform == RuntimePlatform.WSAPlayerX64) return "[WIN8]";
		else if(Application.platform == RuntimePlatform.WSAPlayerX86) return "[WIN8]";
		else if(Application.platform == RuntimePlatform.WindowsPlayer) return "[WIN]";
		else if(Application.platform == RuntimePlatform.WindowsWebPlayer) return "[WIN]";
		else if(Application.platform == RuntimePlatform.OSXWebPlayer) return "[MAC]";
		else if(Application.platform == RuntimePlatform.LinuxPlayer) return "[LNX]";
		else if(Application.platform == RuntimePlatform.OSXEditor) return "[MAC]";
		else if(Application.platform == RuntimePlatform.WP8Player) return "[WP8]";
		else return "[???]";
	}
	
	
}
#endif
