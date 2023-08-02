using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBGetNickController : MonoBehaviour {

   public GameObject baseMenuWindow;
	public GameObject panelNickName;
	public GameObject ChildMessageInGetNickName;
	public GameObject gotInternetErrorlWindow;
	
	private const string phpScriptsPrefixPath =  "http://www.blabserver.net/apps/casinohorsegame/";
	
	private const string checkForNickURL = phpScriptsPrefixPath + "chech_for_nick.php";
	private const string insertNickURL = phpScriptsPrefixPath + "insert_nick.php";
	
	
	bool canInsert = false;
	

	// Use this for initialization
	void Start () {
		baseMenuWindow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void gotSaveNickButtom() {
		
		
		InputField imp = GameObject.Find("NickINPUTField").GetComponent<InputField>();
		
		string nik = imp.text;
		
		Debug.Log("************* gotSaveNickButtom *************** : " + nik);
		int index_ = 0;
		for (int i = 1; i < 13; i++) {
			index_ = nik.IndexOf(':');
			if (index_ != -1)
			{
				nik = nik.Remove(index_, 1); // Use integer from IndexOf.
			}
		}
		Debug.Log("************* gotSaveNickButtom *************** : " + nik + " index_ : " + index_);
		
						
		if( string.IsNullOrEmpty(nik) ) {
			ChildMessageInGetNickName.SetActive(true);
		 } else {
		
		   StartCoroutine(postNickname(nik));
		   
		 }  
	}
	
	
	void checkForNickname(){
		panelNickName.SetActive(true);
	}
	
	void hideAlertNoGoodNick() {
		ChildMessageInGetNickName.SetActive(false);
	}
	
	IEnumerator postNickname(string nickName) {
		
		Debug.Log(nickName);
		//Anonymous
		if( (string.IsNullOrEmpty(nickName)) || nickName.Contains("Anonymous")) {
			panelNickName.SetActive(false);
			baseMenuWindow.SetActive(true);
			PlayerPrefs.SetString("PlayerNickName","");
			yield return null;
			
		} else {
			
			if( (string.IsNullOrEmpty(nickName)) || nickName.Contains("Anonymous")) {
				panelNickName.SetActive(false);
				baseMenuWindow.SetActive(true);
				PlayerPrefs.SetString("PlayerNickName","");
				yield return null;
				
			}
			
			string countryCode = PlayerPrefs.GetString("countryCode");
			
			if(string.IsNullOrEmpty(countryCode)) {
				countryCode = "XX";
			} else {
				
			}
			
			canInsert = false;
			
			string newURL = checkForNickURL;
			
			
			WWWForm form = new WWWForm();
			form.AddField("nickname",nickName);
			
			WWW www = new WWW(newURL, form);
			
			yield return www;
			
			if(!string.IsNullOrEmpty(www.error)) {
				print("There was an error posting the high score: " + www.error + " : " + newURL);
				panelNickName.SetActive(false);
				gotInternetErrorlWindow.SetActive(true);
				
			} else {
				if(www.text.Contains("0")) {
					canInsert = true;
				} else {
					
					// messaggio cambia nick
					canInsert = false;
					
					ChildMessageInGetNickName.SetActive(true);
					Invoke("hideAlertNoGoodNick",5);
					yield return null;
					
					
				}
				print("OK : " + www.text);
				
			}
			
			if(canInsert) {
				
				newURL = insertNickURL;
				
				WWWForm form2 = new WWWForm();
				form2.AddField("name",nickName);
				form2.AddField("country",countryCode);
				
				WWW www2 = new WWW(newURL, form2);
				
				yield return www2;
				
				if(!string.IsNullOrEmpty(www2.error)) {
					panelNickName.SetActive(false);
					baseMenuWindow.SetActive(true);
					print("There was an error posting the high score: " + www2.error);
					PlayerPrefs.DeleteKey("PlayerNickName");
				} else {
					
					print("OK : " + www2.text);
					panelNickName.SetActive(false);
					baseMenuWindow.SetActive(true);
					PlayerPrefs.SetString("PlayerNickName",nickName);
					
				}
				
				
			}
		}
		
	}
	
	
	
}
