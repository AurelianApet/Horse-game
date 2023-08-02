using UnityEngine;
using System.Collections;
using System;

public class BBHorsesRaceMoneyControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void executeEndRaceMoneyResult(float res) {
	
	float tmpVal = 0;
	
	 GameObject[] chipOnTableNotLost = GameObject.FindGameObjectsWithTag("betChip");
	 foreach(GameObject g in chipOnTableNotLost) {
 	   tmpVal += g.GetComponent<BBChipData>().betValue;
	 }
	
	    res += tmpVal;
	
		GetComponent<BBGameControllerHorsesRace>().currentCash += res;
		
		GetComponent<BBGameControllerHorsesRace>().TextMyCash.text = String.Format("{0:0,0}", GetComponent<BBGameControllerHorsesRace>().currentCash) + " $";
		
		
		if(BBStaticVariable.currentlevelType == BBStaticVariable.LevelType.normal) {
		  PlayerPrefs.SetFloat("absoluteMaxMoneyWon",GetComponent<BBGameControllerHorsesRace>().currentCash);
			Debug.Log("#################### executeEndRaceMoneyResult ################## : " + res + " : " + PlayerPrefs.GetFloat("absoluteMaxMoneyWon"));
            string[] tables = new string[2];
			tables = BBStaticVariable.getTablenameForNormalTypeGame();		
				StartCoroutine( BBScoreController.executePostScore(GetComponent<BBGameControllerHorsesRace>().currentCash,tables[0]) );
			    StartCoroutine( BBScoreController.executePostScore(res,tables[1]) );

				} else {
			if(GetComponent<BBGameControllerHorsesRace>().currentCash < 100) {
				Debug.Log("#################### executeEndRaceMoneyResult #####  MATCH LOST  ############# : " + res + " : " + GetComponent<BBGameControllerHorsesRace>().currentCash);
				GetComponent<BBGameControllerHorsesRace>().WindowMatchResult.SetActive(true);
			} else {
				StartCoroutine( BBScoreController.executePostScore(GetComponent<BBGameControllerHorsesRace>().currentCash,BBStaticVariable.getTablename(BBStaticVariable.currentlevelType,BBStaticVariable.currentLevelDifficulty,BBStaticVariable.currentLevelTypeNormal)) );
				Debug.Log("#################### executeEndRaceMoneyResult ################## : " + res + " : " + GetComponent<BBGameControllerHorsesRace>().currentCash);
			}
		}
		
		
	
	}
	
}
