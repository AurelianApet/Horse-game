using UnityEngine;
using System.Collections;
using System;

public class BBStaticVariable {

    private static bool wantBBLog = false;

	public enum LevelType {normal,match}
	public static LevelType levelType;
	public static LevelType currentlevelType;
	
	public enum LevelDifficulty {easy,medium,hard,insane}
	public static LevelDifficulty levelDifficulty;
	public static LevelDifficulty currentLevelDifficulty;

	public enum LevelTypeNormal {maxEver,maxMatch}
	public static LevelTypeNormal levelTypeNormal;
	public static LevelTypeNormal currentLevelTypeNormal;
	

   public enum RunnerType {horse,bot,human}
   public static RunnerType runnerType;
   public static RunnerType currentRunnerType;
  
#if USE_PHOTON  
	public enum RunnerToExecuteInScene {robot,dude,horse,teddybig,none}
	public static RunnerToExecuteInScene runnerToExecuteInScene = RunnerToExecuteInScene.robot;
	
#else
	public enum RunnerToExecuteInScene {robot,dude,horse,teddybig,none}
	public static RunnerToExecuteInScene runnerToExecuteInScene = RunnerToExecuteInScene.robot;
#endif
   
	public const string globalPhpScriptsPrefixPath = "http://www.blabserver.net/apps/casinohorsegame/";
   
   public const float playerInitialMoney = 100000;
   public const float playerMINMoneyToRefund = 10000;
   
    public const float moneyOnBuyCoinsSelectio_0 = 5000;
	public const float moneyOnBuyCoinsSelectio_1 = 10000;
	public const float moneyOnBuyCoinsSelectio_2 = 50000;
	
	
   
    public const float cashOnMatchEASY = 1000;
	public const float cashOnMatchNORMAL = 600;
	public const float cashOnMatchHARD = 400;
	public const float cashOnMatchINSANE = 200;
	
	

	public const string SingleScenename = "SinglePlayerRace";
	
	public static string line_1_runnerName = "";
	public static string line_2_runnerName = "";
	public static string line_3_runnerName = "";
	public static string line_4_runnerName = "";
	public static string line_5_runnerName = "";
	public static string line_6_runnerName = "";
	
	// const runners name
	private const string horse_line_1_runnerName = "H_Ultimus";
	private const string horse_line_2_runnerName = "H_Biscuit";
	private const string horse_line_3_runnerName = "H_Fiorella";
	private const string horse_line_4_runnerName = "H_Tradittore";
	private const string horse_line_5_runnerName = "H_Hetrugo";
	private const string horse_line_6_runnerName = "H_Gibson";
	
	private const string bot_line_1_runnerName = "B_Ultimus";
	private const string bot_line_2_runnerName = "B_Biscuit";
	private const string bot_line_3_runnerName = "B_Fiorella";
	private const string bot_line_4_runnerName = "B_Tradittore";
	private const string bot_line_5_runnerName = "B_Hetrugo";
	private const string bot_line_6_runnerName = "B_Gibson";

	private const string Human_line_1_runnerName = "HU_B_Ultimus";
	private const string Human_line_2_runnerName = "HU_Biscuit";
	private const string Human_line_3_runnerName = "HU_Fiorella";
	private const string Human_line_4_runnerName = "HU_Tradittore";
	private const string Human_line_5_runnerName = "HU_Hetrugo";
	private const string Human_line_6_runnerName = "HU_Gibson";
	
	public const string scoreTableName_EASY = "scores_match_easy";
	public const string scoreTableName_NORMAL = "scores_match_normal";
	public const string scoreTableName_HARD = "scores_match_hard";
	public const string scoreTableName_INSANE = "scores_match_insane";
	public const string scoreTableName_MAXTOTAL = " scores_match_normal";
	public const string scoreTableName_MAXMATCH = "scores_match_maxnormalmatch";
	

	public static string[] getTablenameForNormalTypeGame() {
	    string[] res = new string[2];
		res[0] = scoreTableName_MAXTOTAL;
		res[1] = scoreTableName_MAXMATCH;
		
		return res;
	}	
	
	public static string getTablename(LevelType typeCode, LevelDifficulty difficultyCode, LevelTypeNormal normalTypeCode) {
	
	string tmpRes = "";
	
			switch(typeCode) { 
			  case LevelType.match:  //match game
			      switch(difficultyCode) {
			            case LevelDifficulty.easy: tmpRes = scoreTableName_EASY; break;
						case LevelDifficulty.medium: tmpRes = scoreTableName_NORMAL; break;
						case LevelDifficulty.hard: tmpRes = scoreTableName_HARD; break;
						case LevelDifficulty.insane: tmpRes = scoreTableName_INSANE; break;
			      }
			   break; 
			  case LevelType.normal: 
			    switch(normalTypeCode) {
						case LevelTypeNormal.maxEver: tmpRes = scoreTableName_MAXTOTAL; break;
			            case LevelTypeNormal.maxMatch: tmpRes = scoreTableName_MAXMATCH; break;
				}
			break; 
			  }
			  
			  return tmpRes;
	
	}
	
	public static void setGameTypeAndDifficulty(int typeCode, int difficultyCode, int normalCode) {
	
	  switch(typeCode) { 
	        case 0: 
	           currentlevelType = LevelType.match;
				switch(difficultyCode) {
				case 0: currentLevelDifficulty = LevelDifficulty.easy; break;
				case 1: currentLevelDifficulty = LevelDifficulty.medium; break;
				case 2: currentLevelDifficulty = LevelDifficulty.hard; break;
				case 3: currentLevelDifficulty = LevelDifficulty.insane; break;
				}
			break; 
	         case 1: 
	           currentlevelType = LevelType.normal;
	           switch(normalCode) {
	            case 0: currentLevelTypeNormal = LevelTypeNormal.maxEver; break;
			    case 1: currentLevelTypeNormal = LevelTypeNormal.maxMatch; break;
	           }
	            break; 
	            
	         }
	  
	  
	  
	  
		Debug.Log("[BBStaticVariable][setGameTypeAndDifficulty] gameType/Difficulty : " + currentlevelType + "/" + currentLevelDifficulty);
	
	}
	
    public static string getRunnerName(int lineCode, RunnerType _type) {
    
    string retVal = "";
       switch(_type) {
        case RunnerType.bot:
                  switch(lineCode) {
			            case 1:  BBStaticVariable.line_1_runnerName = bot_line_1_runnerName;  retVal = BBStaticVariable.line_1_runnerName; break;
						case 2:  BBStaticVariable.line_2_runnerName = bot_line_2_runnerName;  retVal = BBStaticVariable.line_2_runnerName; break;
						case 3:  BBStaticVariable.line_3_runnerName = bot_line_3_runnerName;  retVal = BBStaticVariable.line_3_runnerName; break;
						case 4:  BBStaticVariable.line_4_runnerName = bot_line_4_runnerName;  retVal = BBStaticVariable.line_4_runnerName; break;
						case 5:  BBStaticVariable.line_5_runnerName = bot_line_5_runnerName;  retVal = BBStaticVariable.line_5_runnerName; break;
						case 6:  BBStaticVariable.line_6_runnerName = bot_line_6_runnerName;  retVal = BBStaticVariable.line_6_runnerName; break;
                        }
		 break;
		case RunnerType.horse:
						switch(lineCode) {
						case 1:  BBStaticVariable.line_1_runnerName = horse_line_1_runnerName;  retVal = BBStaticVariable.line_1_runnerName; break;
						case 2:  BBStaticVariable.line_2_runnerName = horse_line_2_runnerName;  retVal = BBStaticVariable.line_2_runnerName; break;
						case 3:  BBStaticVariable.line_3_runnerName = horse_line_3_runnerName;  retVal = BBStaticVariable.line_3_runnerName; break;
						case 4:  BBStaticVariable.line_4_runnerName = horse_line_4_runnerName;  retVal = BBStaticVariable.line_4_runnerName; break;
						case 5:  BBStaticVariable.line_5_runnerName = horse_line_5_runnerName;  retVal = BBStaticVariable.line_5_runnerName; break;
						case 6:  BBStaticVariable.line_6_runnerName = horse_line_6_runnerName;  retVal = BBStaticVariable.line_6_runnerName; break;
						}
			break;
		case RunnerType.human:
						switch(lineCode) {
						case 1:  BBStaticVariable.line_1_runnerName = Human_line_1_runnerName;  retVal = BBStaticVariable.line_1_runnerName; break;
						case 2:  BBStaticVariable.line_2_runnerName = Human_line_2_runnerName;  retVal = BBStaticVariable.line_2_runnerName; break;
						case 3:  BBStaticVariable.line_3_runnerName = Human_line_3_runnerName;  retVal = BBStaticVariable.line_3_runnerName; break;
						case 4:  BBStaticVariable.line_4_runnerName = Human_line_4_runnerName;  retVal = BBStaticVariable.line_4_runnerName; break;
						case 5:  BBStaticVariable.line_5_runnerName = Human_line_5_runnerName;  retVal = BBStaticVariable.line_5_runnerName; break;
						case 6:  BBStaticVariable.line_6_runnerName = Human_line_6_runnerName;  retVal = BBStaticVariable.line_6_runnerName; break;
						}
			break;
		}
    return retVal;
    
    }
    
	public static IEnumerator GetCountryCodeViaIP() {
		string countryCode = "XX";
		string url =  globalPhpScriptsPrefixPath + "get_ip_code.php";// "http://www.blabserver.net/apps/casinohorsegame/get_ip_code.php";
		
		#if UNITY_EDITOR
		string debugUseThisIP = "";
		if(debugUseThisIP != "") url += "?ip=" + debugUseThisIP;
		#endif
		WWW www = new WWW(url);
		float startTime = Time.time;
		
		Debug.Log("GetCountryCodeViaIP : " + url);
		
		// Wait for download to complete
		while(!www.isDone) {
			if(www.error != null || Time.time - startTime > 8.0f) break; 
			yield return new WaitForSeconds(0.2f);
		}
		
		if (www.error != null) { 
			Debug.Log(www.error); 
			countryCode = "XX";
			PlayerPrefs.SetString("countryCode", countryCode);
		} else {
			Debug.Log(www.error); 
			
		}
		
		if(www.isDone && www.error == null && www.text != null) { 
			
			
			//countryCode = www.text.Substring(www.text.IndexOf("countryCode") + 11 + 3, 2);
			countryCode = www.text.Substring(www.text.IndexOf("countryCode") + 17, 2);
			
			Debug.Log ("CountryCode from IP: " + countryCode);
			
			PlayerPrefs.SetString("countryCode", countryCode);
		} else { 
			countryCode = "XX";
			PlayerPrefs.SetString("countryCode", countryCode);
		}		
	}
	
         
	public static Texture getTextureByCountryCode(string code) {
		Texture2D tex = null;
		
		if(code == "XX" || string.IsNullOrEmpty(code)) {
			tex = Resources.Load("NULL") as Texture2D;
		} else {
			tex = Resources.Load(code) as Texture2D;
		}
		
		return tex;
		
	}
	
	public static Vector2 GetAspectRatio(int x, int y){
		float f = (float)x / (float)y;
		int i = 0;
		while(true){
			i++;
			if(System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				break;
		}
		return new Vector2((float)System.Math.Round(f * i, 2), i);
	}
	
	public static void BBLog(string val) {

		if(!wantBBLog) {

		} else {
  		  Debug.Log(System.DateTime.Now + " : " + val);
  		}
	}

}
