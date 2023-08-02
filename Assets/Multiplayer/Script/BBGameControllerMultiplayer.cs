using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if USE_PHOTON
using Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
#endif

#if USE_PHOTON
public class BBGameControllerMultiplayer : PunBehaviour {
#else
	public class BBGameControllerMultiplayer : MonoBehaviour {
#endif

#if USE_PHOTON
		

		
   [Header ("Must be located in Resources folder")] 
	public Transform playerPrefab;
	
    public Transform[] spawnPointList;
    public GameObject[] playersDataOnTable;
    
    public PhotonView pv;
    
		BBGameControllerHorsesRaceMultiplayer _BBGameControllerHorsesRaceMultiplayer;
	
	void Awake() {
		_BBGameControllerHorsesRaceMultiplayer = GameObject.FindGameObjectWithTag("GameController").GetComponent<BBGameControllerHorsesRaceMultiplayer>();
	}
     
    void instantiateChip(string code) {
    
			BBStaticVariable.BBLog("hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh instantiateChip : " + code);
			
			string[] res = code.Split(new char[] { '_' });
            Vector3 pos = Vector3.zero;
			string newName = "";
			int posID = int.Parse( res[2] );
            
            GameObject[] payPosList = GameObject.FindGameObjectsWithTag("payPos");
            foreach (GameObject g in payPosList) {
               if(g.name == code) { 
                  pos = g.transform.position;
                  newName = code;
                  break;
               }
            }  
    
			GameObject chip = Instantiate(Resources.Load("BetPositionChipMultiplayer"),pos,Quaternion.identity)  as GameObject;
			chip.name = newName;
			chip.GetComponent<BBChipData>().relatedPlayerPositition = int.Parse(res[2]);
			chip.transform.FindChild("valueCanvas/Canvas/ResultValue").GetComponent<Text>().text = "?";
			chip.GetComponent<BBChipData>().canRemoveChip = false;
			chip.GetComponent<BBChipData>().meshRend.material = chip.GetComponent<BBChipData>().multiplayerMatList[posID];
    } 
     
    void checkForChipsOnTable() {
    
	    string[] clist = (string[])PhotonNetwork.room.customProperties["chipsList"];
			//foreach(string s in clist) Debug.Log("cList : " + s);
      
        int counter = 0;
        foreach(string s in clist) {
         BBStaticVariable.BBLog("cList : " + counter + " : " + s);
         counter++;
         
           if(s != "0") {
             instantiateChip(s);
           } else {
            
           }
         
        }
     
    } 
     
	// Use this for initialization
	void Start () {
	
			checkForChipsOnTable();
			BBStaticVariable.BBLog("[MPInSceneController][Start] ***runnerstype*** : " + (int)PhotonNetwork.room.customProperties["runnerstype"]);
		
		    BBStaticVariable.BBLog("[MPInSceneController][Start] Network : " + PhotonNetwork.connectedAndReady 
		          + " : " + PhotonNetwork.room.name
		          + " : " + PhotonNetwork.player.name
		          + " : " + PhotonNetwork.room.maxPlayers
		          + " : " + PhotonNetwork.room.playerCount
		          
		          );
		         
		        if(GetComponent<PhotonView>().isMine) {
			      CreatePlayerObject();
		        }   
		
	     if(PhotonNetwork.isMasterClient) {
		} else {
		    _BBGameControllerHorsesRaceMultiplayer.ButtonStartrace.gameObject.SetActive(false);
			_BBGameControllerHorsesRaceMultiplayer.ButtonNewRace.gameObject.SetActive(false);
				
		 }
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Escape)) {
			PhotonNetwork.Disconnect();
		}
		
		
		
	
	}
	
	void gotButtonClick(GameObject _go) {
		
		switch(_go.name) {
		case "ButtonSTART_RACE":
		
		  if(PhotonNetwork.room.playerCount > 1) {
			   _go.SetActive(false);
			  
		  } else {
				GameObject.Find("ChatUIConMoveingWindow").SendMessage("gotHideShowContailnerButtonForceShow",SendMessageOptions.DontRequireReceiver);
				
				GameObject.Find("ChatComplete").SendMessage("sendExternalmessage","To start race, need at least two players!",SendMessageOptions.DontRequireReceiver);
		  }
			break;
		}
		
	}
	
	
	
	
	public override void OnJoinedRoom()
	{
		CreatePlayerObject();
	}
	
	private void CreatePlayerObject()
	{
	int spawnPos = 1;
	      switch(PhotonNetwork.room.playerCount) {
	        case 1: spawnPos = 1; break;
			case 2: spawnPos = 2; break;
			case 3: spawnPos = 3; break;
			case 4: spawnPos = 4; break;
			case 5: spawnPos = 5; break;
				
	      }
	      
	      int posCounter = 0;
	      foreach(PhotonPlayer pp in PhotonNetwork.playerList) {
	      
				//BBStaticVariable.BBLog("PhotonNetwork.playerList -----------******************************-------------->> : " + pp.name + " isLocal : " + pp.isLocal);
				
				if(pp.customProperties["p_pos"] != null) {
				   posCounter++;
					BBStaticVariable.BBLog("PhotonNetwork.playerList ----NOT NULL -------******************************-------------->> : " + pp.name + " posCounter : " + posCounter);
				} else {
					BBStaticVariable.BBLog("PhotonNetwork.playerList ----NULL NULL NULL -------******************************-------------->> : " + pp.name + " posCounter : " + posCounter);
					if(posCounter == 0) { 
					      pp.SetCustomProperties(new Hashtable(){{"p_pos", 1 }});
					} else {
						posCounter++;
						pp.SetCustomProperties(new Hashtable(){{"p_pos", posCounter }});
					}
				}
	      }  
	      
			for(int x = 0; x < PhotonNetwork.playerList.Length; x++) {
				BBStaticVariable.BBLog("PhotonNetwork.playerList -----------**********CustomProperties********************-------------->> : " + PhotonNetwork.playerList[x].name + " pos : " + PhotonNetwork.playerList[x].customProperties["p_pos"]);
			 } 
	   
	   
	   
	   
       		
			PhotonNetwork.Instantiate( playerPrefab.gameObject.name, spawnPointList[spawnPos].position, spawnPointList[spawnPos].rotation, 0 );
		
			_BBGameControllerHorsesRaceMultiplayer.myPositionOnTable = spawnPos;
	}
	
	
new	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) { 
		
			BBStaticVariable.BBLog("OnPhotonPlayerDisconnected ------------- otherPlayer : " + otherPlayer.name + " is Master : " + otherPlayer.isMasterClient);
			
		
		    
			int disconnectPos = (int)otherPlayer.customProperties["p_pos"];		
			playersDataOnTable[disconnectPos].SetActive(false);
			
			removeChipsPlayerRelated(disconnectPos);
			
			foreach(PhotonPlayer pp in PhotonNetwork.playerList) {
				BBStaticVariable.BBLog("OnPhotonPlayerDisconnected ------------------->>>>>>>>>>>>>>>>>> : " + pp.name + " isMaster : " + pp.isMasterClient);
				if(pp.isMasterClient) {
				   _BBGameControllerHorsesRaceMultiplayer.TextMasterPlayerIndication.text = "Master Player : " + pp.name;
				   
					_BBGameControllerHorsesRaceMultiplayer.MasterCanStartRaceRoot.SetActive(false);
						
				   if(BBStaticVariableMultiplayer.gameState == BBStaticVariableMultiplayer.GameState.waitingForBet) {
				     _BBGameControllerHorsesRaceMultiplayer.ButtonStartrace.SetActive(true);
					 _BBGameControllerHorsesRaceMultiplayer.ButtonNewRace.SetActive(false);
				   }
				    if(BBStaticVariableMultiplayer.gameState == BBStaticVariableMultiplayer.GameState.waitingForNewRace) {
				     _BBGameControllerHorsesRaceMultiplayer.ButtonNewRace.SetActive(true);
					 _BBGameControllerHorsesRaceMultiplayer.ButtonStartrace.SetActive(false);
				   }
				   if(BBStaticVariableMultiplayer.gameState == BBStaticVariableMultiplayer.GameState.raceOnGoing) {
						_BBGameControllerHorsesRaceMultiplayer.ButtonNewRace.SetActive(false);
						_BBGameControllerHorsesRaceMultiplayer.ButtonStartrace.SetActive(false);
				   }
				} else {
					_BBGameControllerHorsesRaceMultiplayer.ButtonNewRace.SetActive(false);
					_BBGameControllerHorsesRaceMultiplayer.ButtonStartrace.SetActive(false);
					_BBGameControllerHorsesRaceMultiplayer.MasterCanStartRaceRoot.SetActive(true);
				}
			}
			
			
	}
	
	void removeChipsPlayerRelated(int posID) {
	   
	   GameObject[] allChips = GameObject.FindGameObjectsWithTag("betChip");
	   
	   foreach(GameObject g in allChips) {
	       if(g.GetComponent<BBChipData>().relatedPlayerPositition == posID) {
	         Destroy(g);
	       }
	   }
	   
	   
	}
	
	void OnApplicationPause(bool pauseStatus) {
	
		  if(pauseStatus) {
		    PhotonNetwork.Disconnect();
		  }
	}
	
	new void OnDisconnectedFromPhoton() {
			BBStaticVariable.BBLog("BBGameControllerMultiplayer -> OnDisconnectedFromPhoton");
	        SceneManager.LoadScene(0);  
	  
	 }
	
	public void messagingHubMP(Color col, string message) {

/*				
		BBStaticVariable.BBLog("**************messagingHubMP***************** : " + message);
		
		string[] oldText = new string[messagingText.Length];
		Color[] oldTextColor = new Color[messagingText.Length];
		
		
		for(int x = 0;x < oldText.Length;x++) {
			oldText[x] = messagingText[x].text;
			oldTextColor[x] = messagingText[x].color;
		}
		
		for(int i = 0; i < messagingText.Length-1; i++) {
			messagingText[i+1].text = oldText[i];
			messagingText[i+1].color = oldTextColor[i];
			
		}
		
		messagingText[0].text = message;
		messagingText[0].color = col;
*/		
		
	}
	
	
#endif	
	
}
