using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BBGameControllerHorsesRace : MonoBehaviour {
 
 public bool isSocialCasinoGame = false;
 public bool useFakeTrisResult = false;
 public GameObject WindowMatchResult;
 public GameObject CameraGame;
 public GameObject Camera3DView;
 public GameObject TextYouAreTheBest;
	
    int FirstPayMultiplier = 5;
	int SecondPayMultiplier = 4;
	int ThirdPayMultiplier = 3;
	int TrisPayMultiplier = 50;
	

    public Text[] raceResult;
	public Text[] raceTRISResult;
	public GameObject[] resultIndicatorList;
    
	
	public float currentCash = 1000000;
	
	public float currentCasinoCash = 1000000;
	
	Button buttBet_100;
	Button buttBet_200;
	Button buttBet_500;
	Button buttBet_1000;
	float selectedMoneyToBet = 100;
	
	
	[HideInInspector]
	Text TextCurrentMoneyBeted;
	[HideInInspector]
	public Text TextMyCash;
	[HideInInspector]
	public Text TextRouletteMessage;
	[HideInInspector]
	public Text TextCasinoCash;
	
	Text TextMoneyLost;
	Text TextMoneyNet;
	Text TextMoneyWon;
	
	
	public RectTransform rectToDrag;
	
	struct MultiMoveChip {public Transform toMove;public Transform destination;};
	
	struct MultiWONMoveChip {public Transform toMove;public Vector3 destination;};
	
	List <MultiMoveChip> multiMoveChipList = new List<MultiMoveChip>();
	bool moveMultiChips = false;
	List <GameObject> tmpLostChipsToNOTRemove = new List<GameObject>();
	
	List <MultiWONMoveChip> multiWONMoveChip = new List<MultiWONMoveChip>();
	bool moveMultiWONChips = false;
	
	public AudioClip clipChipHandle;
	public AudioClip clipTap;
	
    
	public Transform CASINOPOS_CHIPS_CONTAINER;
	public Transform PlayerPayingPosition;
	
	GameObject ButtonStartrace;
	GameObject ButtonNewRace;
	
	public float moveingStepChipsValue = 20;
	
	GameObject HorsesStatsRoot;
	
	// toReset ===========================
	
	bool raceStarted = false;
	bool gotArrriveFirst = false;
	bool gotArrrivesecond = false;
	bool gotArrriveThird = false;
	
	public int[] trisResult; 
	public int[] trisChoice = new int[3]; 
	
	int horsesArriveCounter = 0;
	
	public Vector3[] posToPay = new Vector3[4];
	
	public float currentCashBetted = 0;
	
	float currentRaceEndResult = 0;
	float currentRaceWonResult = 0;
	float currentraceLoseResult = 0;
	
	
	//===============================================
	
	void Awake() {
		#if UNITY_EDITOR
		gameObject.AddComponent<BBGetScreenShoot>();
		#endif   
	}
    
    public void trisDropDownOnChange(GameObject _go) {
    
    int val = _go.GetComponent<Dropdown>().value + 1;
    
		GameObject DropdownTris_1 = GameObject.Find("DropdownTris_1");
		GameObject DropdownTris_2 = GameObject.Find("DropdownTris_2");
		GameObject DropdownTris_3 = GameObject.Find("DropdownTris_3");
	
			
    
		BBStaticVariable.BBLog("trisDropDownOnChange : " + _go.name + " : " + val);
    
      switch(_go.name) {
		case "DropdownTris_1": 
			if( (val == trisChoice[1] || val == trisChoice[2]) && ( val != 7 && val != 6 ) ) {
				_go.GetComponent<Dropdown>().value = 6;
			} else {
				trisChoice[0] = val; 
			}
		break;
		case "DropdownTris_2":
			if((val == trisChoice[0] || val == trisChoice[2]) && ( val != 7 && val != 6 ) ) {
				_go.GetComponent<Dropdown>().value = 6;
			} else {
				trisChoice[1] = val;  
			}
		break;
		case "DropdownTris_3": 
			if((val == trisChoice[0] || val == trisChoice[1]) && ( val != 7 && val != 6 )  ) {
				_go.GetComponent<Dropdown>().value = 6;
			} else {
				trisChoice[2] = val;
			}
			break;
      }
   
		if(DropdownTris_1.GetComponent<Dropdown>().value < 6 && DropdownTris_2.GetComponent<Dropdown>().value < 6 && DropdownTris_3.GetComponent<Dropdown>().value < 6) {
			DropdownTris_1.GetComponent<Image>().color = Color.green;
			DropdownTris_2.GetComponent<Image>().color = Color.green;
			DropdownTris_3.GetComponent<Image>().color = Color.green;
		} else {
			DropdownTris_1.GetComponent<Image>().color = Color.red;
			DropdownTris_2.GetComponent<Image>().color = Color.red;
			DropdownTris_3.GetComponent<Image>().color = Color.red;
		}
        
    }
    
	void gotButtonClick(GameObject _go) {
	
		GetComponent<AudioSource>().PlayOneShot(clipTap);
	
	    switch(_go.name) {
		case "ButtonStartrace":
	         if(haveBetOnTable()) {
	           GameObject[] horses = GameObject.FindGameObjectsWithTag("Player");
			   foreach(GameObject g in horses) g.SendMessage("realStart",SendMessageOptions.DontRequireReceiver); 
				TextRouletteMessage.text = "Good Luck...";
				raceStarted = true;
				_go.GetComponent<Button>().interactable = false;
				GameObject[] betPointList = GameObject.FindGameObjectsWithTag("betObject"); 
				foreach(GameObject g in betPointList) {
					g.GetComponent<BoxCollider>().enabled = false;
					g.GetComponent<MeshRenderer>().enabled = false;
				}
				HorsesStatsRoot.SetActive(false);
	         } else {
				TextRouletteMessage.text = "Please Bet Before Start Race..."; TextRouletteMessage.color = Color.red;
	         }
	    break;
		case "ButtonNewRace":
		  _go.GetComponent<Button>().interactable = false;
	      startNewRace();		
		break;
		case "Button_EXIT":
         SceneManager.LoadScene(0);	    
	    break;
			case "ButtonChangeCamera":
			if(CameraGame.activeSelf){CameraGame.SetActive(false);Camera3DView.SetActive(true);} else {CameraGame.SetActive(true);Camera3DView.SetActive(false);} 
			break;
	    
	    }
	
	}
	
	void startNewRace() {
	
	   GameObject[] toRemoveList  = GameObject.FindGameObjectsWithTag("toRemove");
	   foreach(GameObject g in toRemoveList) Destroy(g);
	
		HorsesStatsRoot.SetActive(true);
	
		currentRaceEndResult = 0; currentRaceWonResult = 0; currentraceLoseResult = 0;
		TextMoneyLost.text = "0"; TextMoneyWon.text = "0"; TextMoneyNet.text = "0";
	
		StartCoroutine(removePayChips());
	
		foreach(GameObject g in resultIndicatorList) {
		  if(g != null) g.SetActive(false);
		}
	
		GameObject[] betPointList = GameObject.FindGameObjectsWithTag("betObject"); 
		foreach(GameObject g in betPointList) {
			g.GetComponent<BoxCollider>().enabled = true;
			g.GetComponent<MeshRenderer>().enabled = true;
		}
		
		GameObject.Find("DropdownTris_1").GetComponent<Dropdown>().value = 6;GameObject.Find("DropdownTris_2").GetComponent<Dropdown>().value = 6;GameObject.Find("DropdownTris_3").GetComponent<Dropdown>().value = 6;
		
		raceTRISResult[1].text = "?";raceTRISResult[2].text = "?";raceTRISResult[3].text = "?";
		
		raceResult[1].text = "?";raceResult[2].text = "?";raceResult[3].text = "?";
		
		 raceStarted = false;
		 gotArrriveFirst = false;
		 gotArrrivesecond = false;
		 gotArrriveThird = false;
		
		for(int i = 0; i < trisResult.Length;i++) trisResult[i] = 0; for(int i = 0; i < trisChoice.Length;i++) trisChoice[i] = 0;
			
		 horsesArriveCounter = 0;
		
		for(int i = 0; i < posToPay.Length;i++) posToPay[i] = Vector3.zero;
		
		 currentCashBetted = 0;
		
		ButtonStartrace.GetComponent<Button>().interactable = true;
		
		GameObject[] horses = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject g in horses) g.transform.position = g.GetComponent<BBHorsesController>().horseStartingPos; 
		
		GameObject[] oldBetChip = GameObject.FindGameObjectsWithTag("betChip");
		foreach(GameObject oc in oldBetChip) Destroy(oc);
		
		showHorsesStats();
		
		TextMyCash.text = String.Format("{0:0,0}", currentCash) + " $";
	}
	
	void paymentHub(float val, bool isLose) {
		BBStaticVariable.BBLog("#####+++++ paymentHub +++++##### ---> : " + val + " isLose : " + isLose);
		
		
		if(isLose) {
			currentraceLoseResult += val;
		} else {
			currentRaceWonResult += val;
		}
		
		currentRaceEndResult = currentRaceWonResult - currentraceLoseResult;
		
		TextMoneyLost.text = currentraceLoseResult.ToString();
		TextMoneyWon.text = currentRaceWonResult.ToString();
		TextMoneyNet.text = currentRaceEndResult.ToString();
		
	}
	
	IEnumerator removePayChips() {
		BBStaticVariable.BBLog("#####+++++####----- removePayChips ----#####+++++##### ---> ");
		
		multiMoveChipList.Clear();
		
		GameObject[] removeList = GameObject.FindGameObjectsWithTag("payChip");
		
		foreach(GameObject g in removeList) {
			MultiMoveChip  multiList = new MultiMoveChip();
			multiList.toMove = g.transform;
			multiList.destination = PlayerPayingPosition;
			multiMoveChipList.Add(multiList);
		}
		
		moveMultiChips = true;	
		
		yield return new WaitForEndOfFrame();
	}
	
	IEnumerator removeLostChips() {
		
		BBStaticVariable.BBLog("#####+++++#### removeLostChips #####+++++##### ---> ");
		
	 multiMoveChipList.Clear();
	 float lostval = 0;
	   
	  GameObject[] removeList = GameObject.FindGameObjectsWithTag("betChip");
	  
	  foreach(GameObject g in removeList) {
	    
			if(tmpLostChipsToNOTRemove.Count > 0) {  
				foreach(GameObject g1 in tmpLostChipsToNOTRemove) {
				    if(g.name == g1.name) {
						
				    } else {
						MultiMoveChip  multiList = new MultiMoveChip();
						multiList.toMove = g.transform;
						multiList.destination = CASINOPOS_CHIPS_CONTAINER;
						multiMoveChipList.Add(multiList);
						g.tag = "toRemove";
				    }
				}
	        } else {
				MultiMoveChip  multiList = new MultiMoveChip();
				multiList.toMove = g.transform;
				multiList.destination = CASINOPOS_CHIPS_CONTAINER;
				multiMoveChipList.Add(multiList);
				g.tag = "toRemove";
			}
			
	        string _name = g.name;	
			if(tmpLostChipsToNOTRemove.Find(s => s.name.Equals(_name)) == null ) {
				lostval += g.GetComponent<BBChipData>().betValue;
				BBStaticVariable.BBLog("lostval<---------------------#####+++++#### removeLostChips #####+++++##### ---> lostval : " + lostval + " : " +  g.GetComponent<BBChipData>().betValue + " : " + g.name);
			} else {
				BBStaticVariable.BBLog("#####+++++#### removeLostChips #####+++++##### ---> lostval : " + lostval + " : " +  g.GetComponent<BBChipData>().betValue + " : " + g.name);
			}
		}
	  
	    paymentHub(lostval,true);
		
		moveMultiChips = true;	
		
	  yield return new WaitForEndOfFrame();
	
	}
	
	IEnumerator executeWONOnMultiPlaces(float[] values,string[] betIdCode,Vector3[] destinationPos) {
		GetComponent<AudioSource>().PlayOneShot(clipChipHandle);
		
		multiWONMoveChip.Clear();
		
		for(int x = 0; x < betIdCode.Length; x++){
		
		  if(betIdCode[x] != null ) {
           		GameObject chip = Instantiate(Resources.Load("BetPositionChip"),CASINOPOS_CHIPS_CONTAINER.transform.position,Quaternion.identity)  as GameObject;
				chip.transform.localScale = new Vector3(2.5f,4,0.18f);
				chip.name = betIdCode[x] + "_PAY"; chip.tag = "payChip";
				chip.GetComponent<BBChipData>().s_posID = betIdCode[x];
				chip.GetComponent<BBChipData>().betValue = values[x];
				chip.GetComponent<BBChipData>().meshRend.material = chip.GetComponent<BBChipData>().doubleMat;
				chip.GetComponent<BBChipData>().resultCanvas.SetActive(true);
				chip.transform.FindChild("valueCanvas/Canvas/ResultValue").gameObject.GetComponent<Text>().text = values[x].ToString();
				
				MultiWONMoveChip  multiList = new MultiWONMoveChip();
				multiList.toMove = chip.transform; multiList.destination = destinationPos[x];
				
				multiWONMoveChip.Add(multiList);
		 }
		}
		
		foreach (MultiWONMoveChip m in multiWONMoveChip) BBStaticVariable.BBLog("-----------------------> : " + m.toMove.gameObject.name + " : " + m.destination);
		
		moveMultiWONChips = true;
		
		yield return new WaitForSeconds(1);
		
		float tmpLoseVal = 0;				
		foreach(float f in values) tmpLoseVal += f;
		paymentHub(tmpLoseVal,false); 
		
		
		
	}
	
    
	// Use this for initialization
	IEnumerator Start () {
		if(!CameraGame) CameraGame = GameObject.FindGameObjectWithTag("MainCamera");
	   
	    if(BBStaticVariable.currentlevelType == BBStaticVariable.LevelType.normal) {
	    
				if(PlayerPrefs.HasKey("absoluteMaxMoneyWon")) {
					currentCash = PlayerPrefs.GetFloat("absoluteMaxMoneyWon");
				} else {
					currentCash = 1000000;
					PlayerPrefs.SetFloat("absoluteMaxMoneyWon",currentCash);
				}
				
			yield return StartCoroutine(BBScoreController.getBestScore(BBStaticVariable.getTablenameForNormalTypeGame()[0], value => {  
				BBStaticVariable.BBLog("getBestScore : " + value[0] + " -> " + value[1] + " -> " + value[2]);
				GameObject.Find("BestCountryRawImage").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(value[2]);
				GameObject.Find("TextBestNick").GetComponent<Text>().text = value[1];
				GameObject.Find("TextBestScore").GetComponent<Text>().text = value[0];
				
				if(value[1] == PlayerPrefs.GetString("PlayerNickName")) {
					TextYouAreTheBest.SetActive(true);
				}
			}));
			
				
	    } else {
	     switch(BBStaticVariable.currentLevelDifficulty) {
	        case BBStaticVariable.LevelDifficulty.easy: currentCash = BBStaticVariable.cashOnMatchEASY; break;
			case BBStaticVariable.LevelDifficulty.medium: currentCash = BBStaticVariable.cashOnMatchNORMAL; break;
			case BBStaticVariable.LevelDifficulty.hard: currentCash = BBStaticVariable.cashOnMatchHARD; break;
			case BBStaticVariable.LevelDifficulty.insane: currentCash = BBStaticVariable.cashOnMatchINSANE; break;
				
	     }

			yield return StartCoroutine(BBScoreController.getBestScore(BBStaticVariable.getTablename(BBStaticVariable.currentlevelType,BBStaticVariable.currentLevelDifficulty,BBStaticVariable.currentLevelTypeNormal), value => {  
				BBStaticVariable.BBLog("getBestScore : " + value[0] + " -> " + value[1] + " -> " + value[2]);
				GameObject.Find("BestCountryRawImage").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(value[2]);
				GameObject.Find("TextBestNick").GetComponent<Text>().text = value[1];
				GameObject.Find("TextBestScore").GetComponent<Text>().text = value[0];
				
				if(value[1] == PlayerPrefs.GetString("PlayerNickName")) {
					TextYouAreTheBest.SetActive(true);
				}
			}));
			
	   }
		
		HorsesStatsRoot = GameObject.Find("HorsesStatsRoot");
	
		TextRouletteMessage = GameObject.Find("TextRouletteMessage").GetComponent<Text>();
		TextRouletteMessage.text = "Make Your Bet Then Start Race...";
		TextMyCash = GameObject.Find("TextMyCash").GetComponent<Text>();
		TextMyCash.text =  String.Format("{0:0,0}", currentCash) + " $"; 
		
		if(isSocialCasinoGame) {
		   TextCasinoCash = GameObject.Find("TextCasinoCash").GetComponent<Text>();
		   TextCasinoCash.text =  String.Format("{0:0,0}", currentCasinoCash) + " $"; 
		} else {
			GameObject.Find("TextCasinoCash").SetActive(false);
			GameObject.Find("ImageCasino").SetActive(false);
		}
		
		TextCurrentMoneyBeted = GameObject.Find("TextCurrentMoneyBeted").GetComponent<Text>();
		
		GameObject.Find("DropdownTris_1").GetComponent<Dropdown>().interactable = false;
		GameObject.Find("DropdownTris_2").GetComponent<Dropdown>().interactable = false;
		GameObject.Find("DropdownTris_3").GetComponent<Dropdown>().interactable = false;
	
		ButtonStartrace = GameObject.Find("ButtonStartrace");	
		ButtonNewRace = GameObject.Find("ButtonNewRace");	
		
		TextMoneyLost = GameObject.Find("TextMoneyLost").GetComponent<Text>();
		TextMoneyNet = GameObject.Find("TextMoneyNet").GetComponent<Text>();
		TextMoneyWon = GameObject.Find("TextMoneyWon").GetComponent<Text>();
		
		showHorsesStats();
		
	}
	
	void FixedUpdate() {
		
		if(moveMultiWONChips) {
			float step = moveingStepChipsValue * Time.deltaTime;
			int notStillMove = multiWONMoveChip.Count;
			//BBStaticVariable.BBLog(notStillMove + " !!!!!!!multiWONMoveChip.Count : " + multiWONMoveChip.Count);
			
			for(int x = 0; x < multiWONMoveChip.Count;x++) {
				
					multiWONMoveChip[x].toMove.position = Vector3.MoveTowards(multiWONMoveChip[x].toMove.position,  multiWONMoveChip[x].destination, step);
					if(multiWONMoveChip[x].toMove.position.x == multiWONMoveChip[x].destination.x) {
						notStillMove--;
					}
			}
			
			if(notStillMove < 1) moveMultiWONChips = false;
		}
		
		if(moveMultiChips) {
			float step = moveingStepChipsValue * Time.deltaTime;
			int notStillMove = multiMoveChipList.Count;
			
			for(int x = 0; x < multiMoveChipList.Count;x++) {
				multiMoveChipList[x].toMove.position = Vector3.MoveTowards(multiMoveChipList[x].toMove.position,  multiMoveChipList[x].destination.position, step);
				if(multiMoveChipList[x].toMove.position.x == multiMoveChipList[x].destination.position.x) {
					notStillMove--;
				}
			}
			
			if(notStillMove < 1) moveMultiChips = false;
			
		}
		
		
	}
	
	void Update() {
		if( Input.GetMouseButtonDown(0))// && !betInProgress )
		{
		  if(raceStarted) return;
		  
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			
			if( Physics.Raycast( ray, out hit, 100 ) )
			{
				if(hit.collider.gameObject.tag == "betObject") {
					BBStaticVariable.BBLog( hit.transform.gameObject.name );
					if(currentCash >= selectedMoneyToBet) {
						GameObject chip = Instantiate(Resources.Load("BetPositionChip"),hit.collider.transform.position,Quaternion.identity)  as GameObject;
						chip.transform.localScale = new Vector3(2.5f,4,0.18f);
						chip.name = hit.transform.gameObject.name;
						chip.GetComponent<BBChipData>().betValue = selectedMoneyToBet;
						if(selectedMoneyToBet == 1000) chip.GetComponent<BBChipData>().meshRend.material = chip.GetComponent<BBChipData>().blueMat;
						if(selectedMoneyToBet == 200) chip.GetComponent<BBChipData>().meshRend.material = chip.GetComponent<BBChipData>().redMat;
						
						currentCash -= selectedMoneyToBet; TextMyCash.text = String.Format("{0:0,0}", currentCash) + " $";
						currentCashBetted += selectedMoneyToBet; TextCurrentMoneyBeted.text = String.Format("{0:0,0}", currentCashBetted) + " $";
						
						if(hit.transform.gameObject.name == "TRIS_BET") {
							GameObject.Find("DropdownTris_1").GetComponent<Dropdown>().interactable = true;
							GameObject.Find("DropdownTris_2").GetComponent<Dropdown>().interactable = true;
							GameObject.Find("DropdownTris_3").GetComponent<Dropdown>().interactable = true;
						}
						GetComponent<AudioSource>().PlayOneShot(clipChipHandle);
					} else {
						TextRouletteMessage.text = "Ops... No Money to Bet!";
					}
				}
				if(hit.collider.gameObject.tag == "betChip") {
					GetComponent<AudioSource>().PlayOneShot(clipChipHandle);
					currentCash += hit.collider.gameObject.GetComponent<BBChipData>().betValue; TextMyCash.text = String.Format("{0:0,0}", currentCash) + " $";
					currentCashBetted -= hit.collider.gameObject.GetComponent<BBChipData>().betValue; TextCurrentMoneyBeted.text = String.Format("{0:0,0}", currentCashBetted) + " $";
					
					Destroy(hit.collider.gameObject);
					if(hit.transform.gameObject.name == "TRIS_BET") {
						GameObject.Find("DropdownTris_1").GetComponent<Dropdown>().interactable = false;
						GameObject.Find("DropdownTris_2").GetComponent<Dropdown>().interactable = false;
						GameObject.Find("DropdownTris_3").GetComponent<Dropdown>().interactable = false;
					}
					
				}
				
			}
		}
		
	}
	
	void gotHorseArrive(GameObject _horseGO) {
		BBStaticVariable.BBLog("gotHorseArrive : " + _horseGO.name);
		_horseGO.SendMessage("stopRace",SendMessageOptions.RequireReceiver);
		
		if(!gotArrriveFirst) {
		   gotArrriveFirst = true;
		   raceResult[1].text = _horseGO.name;
			trisResult[0] = _horseGO.GetComponent<BBHorsesController>().lineCode;
			resultIndicatorList[trisResult[0]].SetActive(true);
			resultIndicatorList[trisResult[0]].GetComponent<Image>().color = Color.green;
			resultIndicatorList[trisResult[0]].transform.FindChild("text").GetComponent<Text>().text = "1";
		} else {
			if(!gotArrrivesecond) {
				gotArrrivesecond = true;
				raceResult[2].text = _horseGO.name;
				trisResult[1] = _horseGO.GetComponent<BBHorsesController>().lineCode;
				resultIndicatorList[trisResult[1]].SetActive(true);
				resultIndicatorList[trisResult[1]].GetComponent<Image>().color = Color.magenta;
				resultIndicatorList[trisResult[1]].transform.FindChild("text").GetComponent<Text>().text = "2";
				
			} else {
				if(!gotArrriveThird) {
					gotArrriveThird = true;
					raceResult[3].text = _horseGO.name;
					trisResult[2] = _horseGO.GetComponent<BBHorsesController>().lineCode;
					resultIndicatorList[trisResult[2]].SetActive(true);
					resultIndicatorList[trisResult[2]].GetComponent<Image>().color = Color.red;
					resultIndicatorList[trisResult[2]].transform.FindChild("text").GetComponent<Text>().text = "3";
					
					
					raceTRISResult[1].text = trisResult[0].ToString();
					raceTRISResult[2].text = trisResult[1].ToString();
					raceTRISResult[3].text = trisResult[2].ToString();
					
					
				}
			}
		}
		
		horsesArriveCounter++;
		
		if(horsesArriveCounter == 6) {
			StartCoroutine(executeRaceResult());
			string[] stats = new string[] {raceResult[1].text,raceResult[2].text,raceResult[3].text}; 
			BBStaticVariable.BBLog("===========================================>> : " + stats[0] + " : " + raceResult[1].text);
			StartCoroutine(ExecuteStats(stats));
		}
		
	}
	
	void showHorsesStats() {
	   
	   GameObject[] stats = GameObject.FindGameObjectsWithTag("Player");
	   
	   int counter = 0;
	   foreach(GameObject g in stats) {
			counter++;
			GameObject root = GameObject.Find("TextWon_line_" + counter.ToString());
			root.GetComponent<Text>().text = PlayerPrefs.GetInt("won_1_" + g.name).ToString(); 
			root.transform.FindChild("TextName").GetComponent<Text>().text = BBStaticVariable.getRunnerName(counter,BBStaticVariable.currentRunnerType);
			
	   }
		
	}
	
	IEnumerator ExecuteStats(string[] val) {
	
	  yield return new WaitForEndOfFrame();
	  
		     if(val[0] == BBStaticVariable.line_1_runnerName) PlayerPrefs.SetInt("won_1_" + BBStaticVariable.line_1_runnerName, (PlayerPrefs.GetInt("won_1_" + BBStaticVariable.line_1_runnerName) + 1) );
		else if(val[0] == BBStaticVariable.line_2_runnerName) PlayerPrefs.SetInt("won_1_" + BBStaticVariable.line_2_runnerName, (PlayerPrefs.GetInt("won_1_" + BBStaticVariable.line_2_runnerName) + 1) );
		else if(val[0] == BBStaticVariable.line_3_runnerName) PlayerPrefs.SetInt("won_1_" + BBStaticVariable.line_3_runnerName, (PlayerPrefs.GetInt("won_1_" + BBStaticVariable.line_3_runnerName) + 1) );
		else if(val[0] == BBStaticVariable.line_4_runnerName) PlayerPrefs.SetInt("won_1_" + BBStaticVariable.line_4_runnerName, (PlayerPrefs.GetInt("won_1_" + BBStaticVariable.line_4_runnerName) + 1) );
		else if(val[0] == BBStaticVariable.line_5_runnerName) PlayerPrefs.SetInt("won_1_" + BBStaticVariable.line_5_runnerName, (PlayerPrefs.GetInt("won_1_" + BBStaticVariable.line_5_runnerName) + 1) );
		else if(val[0] == BBStaticVariable.line_6_runnerName) PlayerPrefs.SetInt("won_1_" + BBStaticVariable.line_6_runnerName, (PlayerPrefs.GetInt("won_1_" + BBStaticVariable.line_6_runnerName) + 1) );
		
		
	}
	
	IEnumerator executeRaceResult() {
	
		BBStaticVariable.BBLog("-------->> executeRaceResult <<---------");
	
	tmpLostChipsToNOTRemove.Clear();		
	
	 GameObject[] wonChips = new GameObject[4];
	 int[] trisRes = new int[3];
	   yield return new WaitForEndOfFrame();
	   
	   GameObject[] chipList = GameObject.FindGameObjectsWithTag("betChip");
	   
	   foreach(GameObject g in chipList) {
			string[] res = g.name.Split(new char[] { '_' });
			BBStaticVariable.BBLog("=====================>> all : " + res[0].ToString() + "_" + res[1].ToString());
			
			if( (trisResult[0].ToString() == res[0]) && (res[1] == "1") ) {
				BBStaticVariable.BBLog("=====================>> WON 1 Position : " + res[1].ToString() + "_" + res[0].ToString());
				posToPay[0] = getPosToPay(res[1].ToString() + "_" + res[0].ToString());
				wonChips[0] = g;
				trisRes[0] = int.Parse(res[0]);
				tmpLostChipsToNOTRemove.Add(g);
			}
			if( (trisResult[1].ToString() == res[0]) && (res[1] == "2") ) {
				BBStaticVariable.BBLog("=====================>> WON 2 Position : " + res[1].ToString() + "_" + res[0].ToString());
				posToPay[1] = getPosToPay(res[1].ToString() + "_" + res[0].ToString());
				wonChips[1] = g;
				trisRes[1] = int.Parse(res[0]);
				tmpLostChipsToNOTRemove.Add(g);
			}
			if( (trisResult[2].ToString() == res[0]) && (res[1] == "3") ) {
				BBStaticVariable.BBLog("=====================>> WON 3 Position : " + res[1].ToString() + "_" + res[0].ToString());
				posToPay[2] = getPosToPay(res[1].ToString() + "_" + res[0].ToString());
				wonChips[2] = g;
				trisRes[2] = int.Parse(res[0]);
				tmpLostChipsToNOTRemove.Add(g);
			}
			
	   }
	   
		if(useFakeTrisResult) { trisResult[0] = trisChoice[0]; trisResult[1] = trisChoice[1]; trisResult[2] = trisChoice[2]; } 
	   
	  GameObject tmpOutGO = null;  
	 if(haveBetOnTris(out tmpOutGO)) {
			if( ( ( trisChoice[0] == trisResult[0]) && (trisChoice[1] == trisResult[1]) && (trisChoice[2] == trisResult[2]) ) && (tmpOutGO != null)  ) {
				BBStaticVariable.BBLog("=====================>> *********** WON TRIS **********");
				wonChips[3] = tmpOutGO;//GameObject.Find("TRIS_BET"); 
				posToPay[3] = GameObject.Find("TRIS_PAY").transform.position; 
				tmpLostChipsToNOTRemove.Add(wonChips[3]);
			} else {
				BBStaticVariable.BBLog("=====================>> ******* NOT **** WON TRIS **** NOT ****** : " + trisChoice[0] + " : " + trisResult[0] + " / " + trisChoice[1] + " : " + trisResult[1] + " / " + trisChoice[2] + " : " + trisResult[2]);
			}
	 } 
	     
		float[] val = new float[wonChips.Length];
		string[] betId = new string[wonChips.Length];
		Vector3[] trans = new Vector3[wonChips.Length];
		
		bool gotWon = false;
			
		   for(int x = 0; x < wonChips.Length;x++) {
			BBStaticVariable.BBLog("-----------------------------1-------------------x---->> : " + x);
		      if(wonChips[x] != null) {
		         switch(x) {
				  case 0: val[x] = wonChips[x].GetComponent<BBChipData>().betValue * FirstPayMultiplier; break;
				  case 1: val[x] = wonChips[x].GetComponent<BBChipData>().betValue * SecondPayMultiplier; break;
				  case 2: val[x] = wonChips[x].GetComponent<BBChipData>().betValue * ThirdPayMultiplier; break;
				  case 3: val[x] = wonChips[x].GetComponent<BBChipData>().betValue * TrisPayMultiplier; break;
					
		         }
			    betId[x] = wonChips[x].name;
				trans[x] = posToPay[x];
				gotWon = true; 
			  }
			}
			
			if(gotWon) {
			   yield return StartCoroutine( executeWONOnMultiPlaces(val,betId,trans) );
			   yield return new WaitForSeconds(5);
			}
		
		    StartCoroutine(removeLostChips());
		    
		    yield return new WaitForSeconds(4);
		    
		ButtonNewRace.GetComponent<Button>().interactable = true;
		
		if(!isSocialCasinoGame) {
		  GetComponent<BBHorsesRaceMoneyControl>().executeEndRaceMoneyResult(currentRaceWonResult);
	    } else {
	    
	    }
	}
	
	Vector3 getPosToPay(string code) {
	
	  GameObject[] posList = GameObject.FindGameObjectsWithTag("payPos");
	  foreach(GameObject g in posList) {
	     if(g.name == code) {
	       return g.transform.position;
	     }
	  }
	
	 return Vector3.zero;
	 
	}
	
	public void OnEndDrag(UnityEngine.EventSystems.BaseEventData eventData)
	{
		BBStaticVariable.BBLog("Got end drag");
		Camera.main.SendMessage("gotDrag",true,SendMessageOptions.DontRequireReceiver);
	}
	
	public void OnDrag(UnityEngine.EventSystems.BaseEventData eventData)
	{
		Camera.main.SendMessage("gotDrag",false,SendMessageOptions.DontRequireReceiver);
		
		var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
		if (pointerData == null) { return; }
		
		
		var currentPosition = rectToDrag.position;
		currentPosition.x += pointerData.delta.x;
		currentPosition.y += pointerData.delta.y;
		rectToDrag.position = currentPosition;
		
	}

	bool haveBetOnTris(out GameObject goRes) {
	
	goRes = null;
	bool tmpRes = false;
	
		GameObject[] t = GameObject.FindGameObjectsWithTag("betChip");
		
		foreach(GameObject g in t) {
			if(g.name == "TRIS_BET") {
			   tmpRes = true;
			   goRes = g;
			 }  
		}
		
		return tmpRes;
	}
			
	bool haveBetOnTable() {
	  GameObject[] betChips = GameObject.FindGameObjectsWithTag("betChip");
	   if(betChips.Length > 0)
	     return true;
	   else 
	     return false;
	}
	
	void gotBetSelection(GameObject _go) {
		
		BBStaticVariable.BBLog(_go.name);
		
		if(!buttBet_100) { 
		    buttBet_100 = GameObject.Find("ButtonBet_100").GetComponent<Button>();
			buttBet_200 = GameObject.Find("ButtonBet_200").GetComponent<Button>();
			buttBet_1000 = GameObject.Find("ButtonBet_1000").GetComponent<Button>();
		}
		
		buttBet_100.GetComponent<Image>().color = Color.white; 
		buttBet_200.GetComponent<Image>().color = Color.white; 
		buttBet_1000.GetComponent<Image>().color = Color.white;	
		
		     if(_go.name == "ButtonBet_100") { selectedMoneyToBet = 100; buttBet_100.GetComponent<Image>().color = Color.yellow; }
		else if(_go.name == "ButtonBet_200") { selectedMoneyToBet = 200; buttBet_200.GetComponent<Image>().color = Color.yellow;}
		else if(_go.name == "ButtonBet_1000") { selectedMoneyToBet = 1000; buttBet_1000.GetComponent<Image>().color = Color.yellow;}
		
	}
	
	
}
