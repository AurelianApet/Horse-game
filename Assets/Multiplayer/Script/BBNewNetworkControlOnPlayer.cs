using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
#if USE_PHOTON	
using Hashtable = ExitGames.Client.Photon.Hashtable;
#endif
public class BBNewNetworkControlOnPlayer : MonoBehaviour {

#if USE_PHOTON	
	public PhotonView pv;
#endif	

	public Color[] multiplayerPlayersColor;

	public MonoBehaviour[] MonoBToDeleteOnNotMine;
	
	public int playerPositionOnTable = 0;
	
	private BBGameControllerMultiplayer _BBGameControllerMultiplayer;

	// Use this for initialization
	void Start () {
#if USE_PHOTON	
		
		_BBGameControllerMultiplayer = GameObject.Find("_GameControllerMultiplayer").GetComponent<BBGameControllerMultiplayer>();
	  
		string countryCode = "";
		
		if(!pv.isMine) {
			playerPositionOnTable = (int) pv.owner.customProperties["p_pos"];
			BBStaticVariableMultiplayer.myCurrentPositionOnTable = playerPositionOnTable;
			
			foreach(MonoBehaviour mb in MonoBToDeleteOnNotMine) mb.enabled = false;
			gameObject.tag = "remote";
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- > ### NOT isMine NOT ### : " + pv.owner.name + " playerPositionOnTable : " + playerPositionOnTable);
			countryCode =  (string)pv.owner.customProperties["pcountry"];
			gameObject.name = pv.owner.name;
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].SetActive(true);
			string spriteToLoad = "playerAvatar_" + ((int)pv.owner.customProperties["avatarCode"]).ToString();
			var sprite = Resources.Load<Sprite>(spriteToLoad);
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("Image").GetComponent<Image>().overrideSprite = sprite;
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- > ### NOT isMine NOT ### COUNTRY CODE: " + countryCode + " avatarCode : " + (int)pv.owner.customProperties["avatarCode"] + " spriteToLoad : " + spriteToLoad + " playerPositionOnTable : " + playerPositionOnTable);
			
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("Textname").GetComponent<Text>().text = gameObject.name;
			
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("ImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode((string)pv.owner.customProperties["pcountry"]);
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("TextCash").GetComponent<Text>().text =  String.Format("{0:0,0}", ((float)pv.owner.customProperties["playerCash"]).ToString()) + " $";
			
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("chipColor").GetComponent<Image>().color = multiplayerPlayersColor[playerPositionOnTable];
			
		} else {
			playerPositionOnTable = (int) PhotonNetwork.player.customProperties["p_pos"];
			transform.position = _BBGameControllerMultiplayer.spawnPointList[playerPositionOnTable].position;	
			gameObject.tag = "PlayerMP";
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- > ### YES isMine YES ### : " + PhotonNetwork.player.name);
			countryCode =  (string)PhotonNetwork.player.customProperties["pcountry"];
			gameObject.name = PhotonNetwork.player.name;
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].SetActive(true);
			string spriteToLoad = "playerAvatar_" + ((int)PhotonNetwork.player.customProperties["avatarCode"]).ToString();
			var sprite = Resources.Load<Sprite>(spriteToLoad);
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("Image").GetComponent<Image>().overrideSprite = sprite;
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- >  ### YES isMine YES ### COUNTRY CODE: " + countryCode + " avatarCode : " + (int)pv.owner.customProperties["avatarCode"] + " spriteToLoad : " + spriteToLoad + " playerPositionOnTable : " + playerPositionOnTable);
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("Textname").GetComponent<Text>().text = gameObject.name;
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("Textname").GetComponent<Text>().color = Color.red;
			string myCash = ((float)PhotonNetwork.player.customProperties["playerCash"]).ToString();
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("ImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode((string)PhotonNetwork.player.customProperties["pcountry"]);
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("TextCash").GetComponent<Text>().text = String.Format("{0:0,0}",myCash ) + " $";
			
			_BBGameControllerMultiplayer.playersDataOnTable[playerPositionOnTable].transform.FindChild("chipColor").GetComponent<Image>().color = multiplayerPlayersColor[playerPositionOnTable];
			
		}
		
			
		
		//_BBRecordingController.worldBestPlayerStruct[x].playerInfoHud = _BBRecordingController.worldBestPlayerStruct[x].player.transform.FindChild("playerInfoHud").gameObject;
		//_BBRecordingController.worldBestPlayerStruct[x].playerInfoHud.transform.FindChild("RawImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(_BBRecordingController.worldBestPlayerStruct[x].worldBestCountry); 
		//_BBRecordingController.worldBestPlayerStruct[x].playerInfoHud.transform.FindChild("TextPlayerName").GetComponent<Text>().text = _BBRecordingController.worldBestPlayerStruct[x].worldBestNick; 
		
		transform.FindChild("playerInfoHud/RawImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(countryCode); 
		transform.FindChild("playerInfoHud/TextPlayerName").GetComponent<Text>().text = gameObject.name; 
		
#endif		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
