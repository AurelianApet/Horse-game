using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class BBScoreController : MonoBehaviour {

	private const string phpScriptsPrefixPath = BBStaticVariable.globalPhpScriptsPrefixPath; //"http://www.blabserver.net/apps/casinohorsegame/";
	private const string URLPostScore = phpScriptsPrefixPath + "postScore_dinamic.php";
	private const string URLGetBestScore = phpScriptsPrefixPath + "getBestScore.php";

     
	
	public static IEnumerator executePostScore(float score, string _table) {
		
		BBStaticVariable.BBLog("***postScoreMessage*** score - saved :" + score + " - ");
		
		string name =  PlayerPrefs.GetString("PlayerNickName");
		
		if(name.Contains("Anonymous")) {
			
		} else {
		
			string country = PlayerPrefs.GetString("countryCode"); 
			if(string.IsNullOrEmpty(country)) {
				country = "XX";
			}
			
			string newURL = URLPostScore;
			if(string.IsNullOrEmpty( newURL )) {
				Debug.LogError("Error during post score url is NULL");
				yield return null;
			}
			
			string _name  = name;
			if(string.IsNullOrEmpty(_name)) {
				_name = "Anonymous";
			}
			
			string _score  = score.ToString();
			
			BBStaticVariable.BBLog("[PostScore][PostScore] name : score : country : URL " + _name + " - " + score + " - " + country + "-" + newURL + "-" + SceneManager.GetActiveScene().name);
			
			
			WWWForm form = new WWWForm();
			form.AddField("name",_name);
			form.AddField("score",_score);
			form.AddField("country",country);
			form.AddField("mytable",_table);
			
			WWW www = new WWW(newURL, form);
			
			yield return www;
			
			if (string.IsNullOrEmpty(www.error)) {
				if(www.text == "done") 
				{
					BBStaticVariable.BBLog("post score OK");
				}
				else 
				{
					BBStaticVariable.BBLog("There was an error posting the high score: " + www.text);
				}
			} else {
				BBStaticVariable.BBLog("There was an error posting the high score: " + www.error);
				
			}
			
		}
		

				
    }	
    
	public static	IEnumerator getBestScore(string _table, System.Action<string[]> retValue) {
		
	
		
		string _url = URLGetBestScore;
		
		BBStaticVariable.BBLog("best score ************ VAL ************ " + _url + " : " + _table);
		
		WWWForm form = new WWWForm();
		
		form.AddField("mytable", _table);
		
		
		WWW www = new WWW(_url,form);
		
		yield return www;
		
		if (!string.IsNullOrEmpty(www.error)) {
			BBStaticVariable.BBLog("best score ************ ERROR ************ " + www.error); // score
		} else {
			
			
			
			
			string worldBestValue = www.text;
			
			if( (!(string.IsNullOrEmpty(worldBestValue))) && (worldBestValue.Length != 3) ) {
				string str = null;
				string[] strArr = null;
				str = worldBestValue;
				char[] splitchar = {':'};
				strArr = str.Split(splitchar);
				string bestScore = strArr[0];
				string bestNick = strArr[1];
				string bestCountry = strArr[2];
				
			    string[] res = new string[3];
				res[0] = bestScore;
				res[1] = bestNick;
				res[2] = bestCountry;
				retValue(res);
				
				BBStaticVariable.BBLog("best score ************ SUCCESS ************ " + www.text + " length : " + www.text.Length + " : " + bestScore + " : " + bestNick + " : " + bestCountry);
				
			} else {
				
				
			}
			
			
			
		}
		
	}
	
   
  	

}
