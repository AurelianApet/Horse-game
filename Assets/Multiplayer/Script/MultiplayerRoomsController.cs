using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#if USE_PHOTON


using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;


public class MultiplayerRoomsController : Photon.MonoBehaviour {

	[System.Serializable]
	public class AllMaps{
		public string mapName;
		public string mapInfo;
		public string mapSceneToLoad;
		public Texture2D mapPreview;
		public Vector2 mapPreviewTexDimention;
		public bool wantBackgroundImageOnButton = true;
		public BBStaticVariableMultiplayer.RunnerToExecuteInScene runners; 
		public BBStaticVariable.RunnerType runnerType;
	}
	public List<AllMaps> allMaps;
	private int currentSelectedMap;
	
	private string gameMode = ""; // future use

  public GameObject MPRoomsAccessUI;
  public GameObject ChooseMapUI;
  
	// Use this for initialization
	void Start () {
		BBStaticVariable.BBLog("MultiplayerRoomsController->Start--------------------------------------------------------------> avatarCode : " + PlayerPrefs.GetInt("MyAvatarCode"));
	 popolateMapsButtons();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void createRoom() {
	
	}
	
	void gotItemMAPRoomClick(GameObject _go) {
	
	
	
		BBStaticVariable.BBLog("[MultiplayerRoomsController]gotItemMAPRoomClick : " + _go.name);
		
		string mapID = _go.transform.FindChild("mapID").GetComponent<Text>().text;
		int i_mapID = int.Parse(mapID);
		
		string newRoomName = "[" + mapID + "]" + BBStaticVariableMultiplayer.currentMPRoomName;
		int maxPlayers = 5;//BBStaticVariableMultiplayer.currentMPmaxPlayerNumber;
		
		BBStaticVariableMultiplayer.runnerToExecuteInScene = allMaps[i_mapID].runners;
		BBStaticVariable.currentRunnerType = allMaps[i_mapID].runnerType;
		
		PhotonNetwork.player.name = BBStaticVariableMultiplayer.currentMPPlayerName;
		Hashtable setMapName = new Hashtable(); 
		setMapName["MapName"] = allMaps[i_mapID].mapName;
		setMapName["mapSceneToLoad"] = allMaps[i_mapID].mapSceneToLoad;;
		setMapName["GameMode"] = gameMode;
		setMapName["runnerstype"] = (int)allMaps[i_mapID].runners;
		
		string[] listOfChip = {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"};
		setMapName["chipsList"] = listOfChip;
		
				
		GameObject ChooseMapUIText = GameObject.Find("ChooseMapUIText");
		ChooseMapUIText.GetComponent<Text>().text = "Loading...";
		ChooseMapUIText.transform.FindChild("Image").GetComponent<Image>().enabled = true;
		
		ChooseMapUI.SetActive(false);
		
		PhotonNetwork.CreateRoom(newRoomName, new RoomOptions() {MaxPlayers = (byte)maxPlayers, CustomRoomProperties = setMapName}, null);
		
		
	}
	
	
	void OnJoinedRoom(){
		print ("Joined room: " + PhotonNetwork.room + " " + PhotonNetwork.masterClient.name + " " + PhotonNetwork.isMasterClient);
		//We joined room, load respective map
		StartCoroutine(LoadMap((string)PhotonNetwork.room.customProperties["mapSceneToLoad"]));
	}
	
	IEnumerator LoadMap(string sceneName){
		
		PhotonNetwork.isMessageQueueRunning = false;

		yield return new WaitForSeconds(1);
		
		BBStaticVariable.BBLog("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH absoluteMaxMoneyWon : " + PlayerPrefs.GetFloat("absoluteMaxMoneyWon"));
		
		
		//PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"pname", PhotonNetwork.player.name}});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"pcountry", PlayerPrefs.GetString("countryCode") }});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"avatarCode", PlayerPrefs.GetInt("MyAvatarCode") }});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"playerCash", PlayerPrefs.GetFloat("absoluteMaxMoneyWon") }});
		//PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"pskill", PlayerPrefs.GetInt("skillRaceDoneCounter") }});
		
		
		
		
		PhotonNetwork.LoadLevel((string)PhotonNetwork.room.customProperties["mapSceneToLoad"]);
		
		BBStaticVariable.BBLog("Loading complete");  
	}
	
	void popolateMapsButtons() {
	
		GameObject MultiplayerMapButtonsRoot = GameObject.Find("PanelBUTTONS_RoomsChooseMap");
		GameObject mapButtonItem = Resources.Load("MultiplayerMapButtonItem") as GameObject;
		
		for(int i = 0; i < allMaps.Count; i++){
		
			GameObject inst = (GameObject)Instantiate(mapButtonItem) as GameObject;
			
			inst.name = allMaps[i].mapSceneToLoad;
			inst.transform.FindChild("UILabelMAPNameOnChooseMapButton").gameObject.GetComponent<Text>().text = allMaps[i].mapName;
			inst.transform.FindChild("UILabelMAPInfoOnChooseMapButton").gameObject.GetComponent<Text>().text = allMaps[i].mapInfo;
			inst.transform.FindChild("MapImage").gameObject.GetComponent<RawImage>().texture = allMaps[i].mapPreview;
			inst.transform.FindChild("MapImage").gameObject.GetComponent<RectTransform>().sizeDelta = allMaps[i].mapPreviewTexDimention;
			inst.GetComponent<Image>().enabled = allMaps[i].wantBackgroundImageOnButton;
			
			inst.transform.FindChild("mapID").gameObject.GetComponent<Text>().text = i.ToString();
			
			inst.transform.SetParent(MultiplayerMapButtonsRoot.transform, false);
			
		}
	
	}
	
	void OnDisconnectedFromPhoton() {
		print ("OnDisconnectedFromPhoton");
	}
	
	void OnConnectedToPhoton(){
		print ("OnConnectedToPhoton : ");
	}
	
	void OnJoinedLobby(){
		print ("OnJoinedLobby");
	}
	
	
}
#endif
