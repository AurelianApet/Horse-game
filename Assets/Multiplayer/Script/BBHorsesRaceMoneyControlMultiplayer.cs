using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

#if USE_PHOTON
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BBHorsesRaceMoneyControlMultiplayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[PunRPC]
	void updateAllPlayersCashView() {
	  
	  foreach(PhotonPlayer pp in PhotonNetwork.playerList) {
			int playerPos = (int) pp.customProperties["p_pos"];
			
			BBStaticVariable.BBLog("[PunRPC][updateAllPlayersCashView] playerPos : " + playerPos + " cash : " + (float) pp.customProperties["playerCash"]);
			float tmpPlayerCash = (float) pp.customProperties["playerCash"];
			
			GameObject.Find("_GameControllerMultiplayer").GetComponent<BBGameControllerMultiplayer>().playersDataOnTable[playerPos].transform.FindChild("TextCash").GetComponent<Text>().text = String.Format("{0:0,0}",tmpPlayerCash.ToString() ) + " $"; 
			
		}
		
	}
	
	public void executeEndRaceMoneyResult(float res) {

/*		
	float tmpVal = 0;
	
	 GameObject[] chipOnTableNotLost = GameObject.FindGameObjectsWithTag("betChip");
	 foreach(GameObject g in chipOnTableNotLost) {
 	   tmpVal += g.GetComponent<BBChipData>().betValue;
	 }
	
	    res += tmpVal;
*/	
		GetComponent<BBGameControllerHorsesRaceMultiplayer>().currentCash += res;
		float tmpCash = GetComponent<BBGameControllerHorsesRaceMultiplayer>().currentCash;
		GetComponent<BBGameControllerHorsesRaceMultiplayer>().TextMyCash.text = String.Format("{0:0,0}", GetComponent<BBGameControllerHorsesRaceMultiplayer>().currentCash) + " $";
		
		
			PlayerPrefs.SetFloat("absoluteMaxMoneyWon",GetComponent<BBGameControllerHorsesRaceMultiplayer>().currentCash);
		BBStaticVariable.BBLog("#################### executeEndRaceMoneyResult ################## : " + res + " : " + PlayerPrefs.GetFloat("absoluteMaxMoneyWon"));
		   
		   PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"playerCash", tmpCash }});
		
		    GetComponent<PhotonView>().RPC("updateAllPlayersCashView",PhotonTargets.All);
		

		
	
	}
	
}
#endif