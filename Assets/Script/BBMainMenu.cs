using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

#if UNITY_IOS && USE_GAME_CENTER	
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.SocialPlatforms;
#endif



public class BBMainMenu : MonoBehaviour {

 public GameObject PanelBuyCoins;
 public GameObject GuideView;
 public GameObject settingsView;
 public GameObject getNickView;
 public GameObject mainMenuView;

 public Toggle[] gameTypeToggleList;
 public Toggle[] gameDifficultyToggleList;
 public GameObject toggleDifficulty;
	
	
 public AudioClip clipTap;
 public GameObject guideScrollView;
 private float currentAbsoluteMoneyCash = 0;
 
 public Dropdown DropDownAvatar;
 	
 public Text TextAbsoluteMoneyWon;


 
	public void onDropDownChange(GameObject _go) {
		
		int val = _go.GetComponent<Dropdown>().value;
		
		switch(_go.name) {
		case "DropdownBuyMoney":
			float currval = PlayerPrefs.GetFloat("absoluteMaxMoneyWon");
		        switch(val) {
		        case 0: //5.000 
				    PlayerPrefs.SetFloat("absoluteMaxMoneyWon", currval + BBStaticVariable.moneyOnBuyCoinsSelectio_0);
		        break;
				case 1: //10.000 
				PlayerPrefs.SetFloat("absoluteMaxMoneyWon", currval + BBStaticVariable.moneyOnBuyCoinsSelectio_1);
					break;
				case 2: //50.000 
				PlayerPrefs.SetFloat("absoluteMaxMoneyWon", currval + BBStaticVariable.moneyOnBuyCoinsSelectio_2);
					break;
			}
			TextAbsoluteMoneyWon.text = String.Format("{0:0,0}", PlayerPrefs.GetFloat("absoluteMaxMoneyWon")) + " $";
			break;
		case "DropDownAvatar":
		    PlayerPrefs.SetInt("MyAvatarCode",val);
			break;
			
		}
		Debug.Log("[BBMainMenuController][onDropDownChange] : " + _go.name + " - " + val + " - avatarCode : " + PlayerPrefs.GetInt("MyAvatarCode"));
	}
	

	public void onChangeToggleGameType(GameObject _go) {
	   if(_go.GetComponent<Toggle>().isOn) {
	        toggleDifficulty.SetActive(false);
	   } else {
			toggleDifficulty.SetActive(true);
	   }
	}

   void Awake() {
   
#if UNITY_EDITOR
 gameObject.AddComponent<BBGetScreenShoot>();
#endif   
   
		if(PlayerPrefs.HasKey("MyAvatarCode")) {
			DropDownAvatar.value =  PlayerPrefs.GetInt("MyAvatarCode");
		}
		
   
		if(PlayerPrefs.HasKey("PlayerNickName")) {
		
		} else {
		  getNickView.SetActive(true);
		  mainMenuView.SetActive(false);
		}
     
		Debug.Log("PlayerNickName : " + PlayerPrefs.GetString("PlayerNickName"));
   
   }
   
/*   
	void OnEnable() {
	
	   if(mainMenuView.activeSelf) {
			TextAbsoluteMoneyWon.text = String.Format("{0:0,0}", currentAbsoluteMoneyCash) + " $";
			if(currentAbsoluteMoneyCash > BBStaticVariable.playerInitialMoney) TextAbsoluteMoneyWon.color = Color.green;
			else TextAbsoluteMoneyWon.color = Color.red;
			
							
			if(currentAbsoluteMoneyCash < BBStaticVariable.playerMINMoneyToRefund) {
				PlayerPrefs.SetFloat("absoluteMaxMoneyWon", BBStaticVariable.playerInitialMoney);
				TextAbsoluteMoneyWon.text = String.Format("{0:0,0}", PlayerPrefs.GetFloat("absoluteMaxMoneyWon")) + " $";
			}		
	   }
	
	}
*/

	// Use this for initialization
	void Start () {
	
#if !USE_PHOTON
		GameObject.Find("ButtonMultiplayerRoot").SetActive(false);
#endif		
	
	StartCoroutine( BBStaticVariable.GetCountryCodeViaIP() );
	
	Screen.sleepTimeout = SleepTimeout.NeverSleep;
	
#if UNITY_ANDROID
#if USE_GPLAY_SERVICE
      if(Application.platform == RuntimePlatform.Android) {
      
			GameObject.Find("_AdMob").SendMessage("BBStart",SendMessageOptions.DontRequireReceiver);
	
			if(!GooglePlayGames.PlayGamesPlatform.Activate().IsAuthenticated()) {
				GooglePlayGames.PlayGamesPlatform.Activate();
				Social.localUser.Authenticate((bool success) => {
					if (success) {
						string token = GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
						Debug.Log(token);
						BBStaticVariable.isPlayerAutenticated = true;
					} else {
					}
				});
			}
	  }
#endif	  
	  
#endif
	    
#if UNITY_IOS	
#if USE_GAME_CENTER			
		Social.localUser.Authenticate (ProcessAuthentication);
#endif	

#if USE_BBIAP							
		Debug.Log("BBCrapsMainMenu -> Start -> showADV : " + PlayerPrefs.GetInt("showADV"));
		if(PlayerPrefs.GetInt("showADV") == 0) { 
			GameObject.Find("_AdMob").SendMessage("BBStart",SendMessageOptions.DontRequireReceiver);
			ButtonBUYRemoveADV.SetActive(true);
		} else {
			ButtonBUYRemoveADV.SetActive(false);
			BBStaticVariable.bannerView.Hide();			
		}
#endif		
#endif

		if(PlayerPrefs.HasKey("absoluteMaxMoneyWon")) {
			currentAbsoluteMoneyCash = PlayerPrefs.GetFloat("absoluteMaxMoneyWon");
			
		} else {
			PlayerPrefs.SetFloat("absoluteMaxMoneyWon", BBStaticVariable.playerInitialMoney);
			currentAbsoluteMoneyCash = PlayerPrefs.GetFloat("absoluteMaxMoneyWon");
			
		}
		
		
		
		TextAbsoluteMoneyWon.text = String.Format("{0:0,0}", currentAbsoluteMoneyCash) + " $";
	}

#if UNITY_IOS && USE_GAME_CENTER	

	void postScore() {
		ReportScore(2000,"iCrapsLBBestScores");
	}
	
	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, checking achievements");
			//Social.ShowLeaderboardUI();
			//Invoke("postScore",30);
		}
		else
			Debug.Log ("Failed to authenticate");
	}
	

	void ReportScore (long score, string leaderboardID) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}
#endif
	
	void setGameTypeAndDifficulty() {
	int gameTypeCode = 0;
	int gameTypeDifficulty = 0;
		
	    for(int t = 0;t < gameTypeToggleList.Length;t++) { if(gameTypeToggleList[t].isOn) gameTypeCode = t;}
		for(int t = 0;t < gameDifficultyToggleList.Length;t++) { if(gameDifficultyToggleList[t].isOn) gameTypeDifficulty = t;}
		
		BBStaticVariable.setGameTypeAndDifficulty(gameTypeCode,gameTypeDifficulty,0);
	
	}
	
	
	void gotButtonClick(GameObject _go) {
		
		GetComponent<AudioSource>().PlayOneShot(clipTap);
		
		switch(_go.name) {
		case "ButtonPlayHorses": 
			    BBStaticVariable.runnerToExecuteInScene = BBStaticVariable.RunnerToExecuteInScene.horse;
			    		        		        
		        BBStaticVariable.currentRunnerType = BBStaticVariable.RunnerType.horse;
		        setGameTypeAndDifficulty(); 
			SceneManager.LoadScene(BBStaticVariable.SingleScenename);
		        break;
		case "ButtonPlayBot": 
			    BBStaticVariable.runnerToExecuteInScene = BBStaticVariable.RunnerToExecuteInScene.robot;
		        BBStaticVariable.currentRunnerType = BBStaticVariable.RunnerType.bot;
		        setGameTypeAndDifficulty(); 
			SceneManager.LoadScene(BBStaticVariable.SingleScenename);
		        break;
		case "ButtonHumanHorses": 
			    BBStaticVariable.runnerToExecuteInScene = BBStaticVariable.RunnerToExecuteInScene.dude;
		        BBStaticVariable.currentRunnerType = BBStaticVariable.RunnerType.human; 
		        setGameTypeAndDifficulty(); 
			SceneManager.LoadScene(BBStaticVariable.SingleScenename);
		        break;
			
		case "ButtonTeddyHorses": 
			BBStaticVariable.runnerToExecuteInScene = BBStaticVariable.RunnerToExecuteInScene.teddybig;
			BBStaticVariable.currentRunnerType = BBStaticVariable.RunnerType.human; 
			setGameTypeAndDifficulty(); 
			SceneManager.LoadScene(BBStaticVariable.SingleScenename);
			break;
			
			
		case "ButtonRanking":
			SceneManager.LoadSceneAsync("ShowRanking");
#if UNITY_IOS && USE_GAME_CENTER			
		Social.ShowLeaderboardUI();
#endif	
#if UNITY_ANDROID && USE_GPLAY_SERVICE
			Social.ShowLeaderboardUI();
#endif
			break;
			case "ButtonAchievements":
#if (UNITY_IOS || UNITY_ANDROID) && (USE_GPLAY_SERVICE || USE_GAME_CENTER)	
	         Social.ShowAchievementsUI();		
#endif	         
				break;	
		   case "ButtonBUYOneMillion":
#if UNITY_IOS && USE_BBIAP		   
			 BBIapController.buyOneMillion();
#endif
			break;
		   case "ButtonBUYRemoveADV":
#if UNITY_IOS && USE_BBIAP		   
			BBIapController.buyRemoveAdv();
#endif			
			break;
			case "ButtonSettings":
			 mainMenuView.SetActive(false);
			 settingsView.SetActive(true);
			break;
		    case "ButtonSettingsExit":
			mainMenuView.SetActive(true);
			settingsView.SetActive(false);
			break;
			case "ButtonGuideExit":
			mainMenuView.SetActive(true);
			GuideView.SetActive(false);
			break;
			case "ButtonGuide":
			mainMenuView.SetActive(false);
			GuideView.SetActive(true);
			break;
			case "ButtonMultiplayer":
			SceneManager.LoadScene("MultiplayerMainMenu");
			break;	
		    case "ButtonSettingsExitOnBuyCoins":
			PanelBuyCoins.SetActive(false);
			mainMenuView.SetActive(true);
			break;	
		    case "ButtonBuyCoinsReal":
			PanelBuyCoins.SetActive(false);
			mainMenuView.SetActive(true);
			break;	
		    case "ButtonBuyCoinsOnMainMenu":
			  PanelBuyCoins.SetActive(true);
			  mainMenuView.SetActive(false);
			break;	
					
		}
	}
	
	
	void checkWaitingForBuyResponceBUYOneMillion(string result) {
#if UNITY_IOS && USE_BBIAP	
		float f_myCash = PlayerPrefs.GetFloat("absoluteMaxMoneyWon");
		
		Debug.Log("checkWaitingForBuyResponceBUYOneMillion --> : " + result);
		
		if(result == "SUCCESS") { //gotBuy million
				f_myCash += 1000000;
				PlayerPrefs.SetFloat("absoluteMaxMoneyWon", f_myCash);
				GameObject.Find("TextAbsoluteMoneyWon").GetComponent<Text>().text = String.Format("{0:0,0}", f_myCash) + " $";
			    ButtonBUYOneMillion.SetActive(false);
	     } else {
					ButtonBUYOneMillion.SetActive(true);
	     } 
#endif
	  
	}
	
	void checkWaitingForBuyResponceBUYRemoveADV(string result) {
#if UNITY_IOS && USE_BBIAP
		Debug.Log("checkWaitingForBuyResponceBUYRemoveADV --> : " + result);
		                   
		    if(result == "SUCCESS") {
				ButtonBUYRemoveADV.SetActive(false);
				PlayerPrefs.SetInt("showADV",1);
				BBStaticVariable.bannerView.Hide();
				Application.LoadLevel(Application.loadedLevel);
			} else {
				ButtonBUYRemoveADV.SetActive(true);
				GameObject.Find("_AdMob").SendMessage("BBStart",SendMessageOptions.DontRequireReceiver);
			}
#endif		 
		 
		  
	}
	
#if UNITY_IOS && USE_BBIAP	
	void enableButtonBUYOneMillion(bool have) {
		
		float f_myCash = PlayerPrefs.GetFloat("absoluteMaxMoneyWon");
		
	   if(!have) {
		   ButtonBUYOneMillion.SetActive(true);
	   } else {
			if(f_myCash < 100) {
				ButtonBUYOneMillion.SetActive(true);
			} else {
				ButtonBUYOneMillion.SetActive(false);
			}
		}
	}

	void enableButtonBUYRemoveADV(bool have) {
	  if(PlayerPrefs.GetInt("showADV") == 0) { 
		if(!have) {
		   ButtonBUYRemoveADV.SetActive(true);
			GameObject.Find("_AdMob").SendMessage("BBStart",SendMessageOptions.DontRequireReceiver);
		 }
	  }
	}
	
	
	
	
#endif
		
}
