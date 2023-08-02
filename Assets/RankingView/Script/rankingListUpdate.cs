using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class rankingListUpdate : MonoBehaviour {

	public Toggle[] gameTypeToggleList;
	public Toggle[] gameDifficultyToggleList;
	public Toggle[] gameNormalToggleList;
	
	public GameObject toggleNormalGame;
	public GameObject toggleMatchGame;
	
	

	public Text labelLoading;

	public GameObject NGUItableRoot;
	public GameObject NGUItableItem;
	public GameObject NGUItableItemForTorneo;
	
	private const string phpScriptsPrefixPath = BBStaticVariable.globalPhpScriptsPrefixPath; //"http://www.blabserver.net/apps/casinohorsegame/";

	private const string phpURLGetScore = phpScriptsPrefixPath + "getHighscore.php";
	

	    Text LabelYourNickName;
	
	public void onChangeToggleGameType(GameObject _go) {
	
		Debug.Log("onChangeToggleGameType : " + _go.name);
	
		if(_go.name == "ToggleGameNormal") {
		  if(_go.GetComponent<Toggle>().isOn) {
				toggleNormalGame.SetActive(true);
				toggleMatchGame.SetActive(false);
		  } else {
				toggleNormalGame.SetActive(false);
				toggleMatchGame.SetActive(true);
			}
		}
	
	}
	
    void Start() {
		
		//showRanking(c_s_tableName);
		
   	}	
	

	void Update() {

		#if UNITY_ANDROID
		if (Input.GetKey(KeyCode.Escape))
		{
		
			Application.LoadLevelAsync(0);

		}
		#endif

	}
	
	void popolateScoreListuGUI(string[] listArray) {
	
		GameObject scoreItem = Resources.Load("uGuiScoreItem") as GameObject;
		GameObject PanelScrollRoot = GameObject.Find("PanelScrollRoot");
	   
		int colorControl = 0;
		
		for (int Bcount = 0; Bcount <= listArray.Length - 1; Bcount++)
		{
			string cc = listArray[Bcount];
			if(cc.Length > 5) {
				int idxDuePunti = cc.IndexOf(":");
				string _cc = cc.Substring(idxDuePunti+1);  
				if(_cc.Length > 2) _cc = "XX";
				
				GameObject inst = (GameObject)Instantiate(scoreItem);
				inst.transform.SetParent(PanelScrollRoot.transform, false);
				
				Text currentLabel = inst.transform.GetComponentInChildren<Text>();
				
				currentLabel.text = listArray[Bcount];
				
				if (colorControl == 0) {currentLabel.color = Color.white; colorControl++;}
				else {currentLabel.color = Color.green; colorControl = 0;}
				
				Texture2D tex;
				if(_cc == "XX") {
					tex = Resources.Load("NULL") as Texture2D;
				} else {
					tex = Resources.Load(_cc) as Texture2D;
				}
				inst.transform.GetComponentInChildren<RawImage>().texture = tex;
			}
			
		} 
	
		RectTransform rt = PanelScrollRoot.GetComponent<RectTransform>();
		
		rt.localPosition = new Vector3(rt.localPosition.y,1536,rt.localPosition.z);
		
	}

	
	void OnEnable () {
		LabelYourNickName = GameObject.Find("LabelYourNickName").GetComponent<Text>();
		LabelYourNickName.text = "Your Nick : " + PlayerPrefs.GetString("PlayerNickName");
		labelLoading = GameObject.Find("LabelLoading").GetComponent<Text>();
		labelLoading.enabled = false;
	}
	
	 public void showRanking(string tableName) {
		
	
		StartCoroutine(GetScore(tableName));
		
	}
	
	
	IEnumerator GetScore(string tableName)
	{
		
		string ascordesc = "D";

		//WindowTitel = "Loading";
		
		WWWForm form = new WWWForm();
		form.AddField("limit","300");
		form.AddField("mytable",tableName);
		form.AddField("ascordesc",ascordesc);



		
		WWW www = new WWW(phpURLGetScore,form);
		yield return www;
		
		if(www.text == "") 
		{
			print("There was an error getting the high score: " + www.error);
			labelLoading.enabled = false;
			
			GameObject scoreItem = Resources.Load("uGuiScoreItem") as GameObject;
			GameObject PanelScrollRoot = GameObject.Find("PanelScrollRoot");
			GameObject inst = (GameObject)Instantiate(scoreItem);
			inst.transform.SetParent(PanelScrollRoot.transform, false);
			inst.transform.GetComponentInChildren<Text>().text = "No Data Found...";
			inst.transform.GetComponentInChildren<Text>().color = Color.red;
			inst.transform.GetComponentInChildren<RawImage>().enabled = false;
			
		}
		else 
		{
			//UILabelMessageTITLE.SetActive(false);
			
			labelLoading.enabled = false;
			string dataString = www.text;
					Debug.Log("data web : " + dataString + "  tablename : " + tableName);

			string str = null;
			string[] strArr = null;
			//int count = 0;
			str = dataString;
			char[] splitchar = {'\n'};
			strArr = str.Split(splitchar);

           popolateScoreListuGUI(strArr);
			
		}
	}

/*
	void popolateRankingListForTorneo(string[] listArray) {
		
		
		
		 GameObject item; //= NGUITools.AddChild(NGUItableRoot, NGUItableItemForTorneo);  
		//item.transform.FindChild("LabelValue").GetComponent<UILabel>().text = "POS-NIK-POINTS-COUNTRY";
		//item.transform.FindChild("img").GetComponent<UITexture>().enabled = false;
		
		int colorControl = 0;
		
		for (int Bcount = 0; Bcount <= listArray.Length - 1; Bcount++)
		{
			//                MessageBox.Show(strArr[count]);
			//Debug.Log(strArr[Bcount]);
			string cc = listArray[Bcount];
			if(cc.Length > 5) {
				int idxDuePunti = cc.IndexOf(":");
				string _cc = cc.Substring(idxDuePunti+1);  
				
				int idxApreParentesi = cc.IndexOf("(");
				string _pos = cc.Substring(idxApreParentesi+1,1);  
				
				//Debug.Log("*************************************_pos =" + _pos); // _cc =: 	XX
				if(_cc.Length > 2) _cc = "XX";
				item = NGUITools.AddChild(NGUItableRoot, NGUItableItemForTorneo);   
				//item.name = roomInfo.name;
				
				UILabel currentLabel = item.transform.FindChild("LabelValue").GetComponent<UILabel>();
				currentLabel.text = listArray[Bcount];
				if (colorControl == 0) {currentLabel.color = Color.white; colorControl++;}
				else {currentLabel.color = Color.green; colorControl = 0;}
				
				Texture2D tex;
				if(_cc == "XX") {
					tex = Resources.Load("NULL") as Texture2D;
				} else {
					tex = Resources.Load(_cc) as Texture2D;
				}
				item.transform.FindChild("img").GetComponent<UITexture>().mainTexture =  tex;
				
				Texture2D texPosition;
				     if(_pos == "1") texPosition = Resources.Load("img_pos_1") as Texture2D;
				else if(_pos == "2") texPosition = Resources.Load("img_pos_2") as Texture2D;
				else if(_pos == "3") texPosition = Resources.Load("img_pos_3") as Texture2D;
				else texPosition = Resources.Load("NULL") as Texture2D;
				
				item.transform.FindChild("imgPosition").GetComponent<UITexture>().mainTexture =  texPosition;
				
				//Debug.Log("tex : " + tex.name);
				
				
			}
			
		} 
		
		//UITable uit = NGUItableRoot.GetComponent<UITable>();
		//uit.Reposition();
		UIGrid uig = NGUItableRoot.GetComponent<UIGrid>();
		uig.Reposition();
		
		
	}
	
*/


	void popolateRankingList(string[] listArray) {

	}
	
	void cleanRanKTable() {
	

		GameObject PanelScrollRoot = GameObject.Find("PanelScrollRoot");
		LayoutElement[] layOE = PanelScrollRoot.GetComponentsInChildren<LayoutElement>();
		
		foreach(LayoutElement l in layOE) {
		  Destroy(l.gameObject);
		}
				
	}


	public void gotSelectedForRanking(string _val, int selectedGroup) {
		
		Button[] PanelBUTTONS_Levels = GameObject.Find("PanelBUTTONS_Levels").transform.GetComponentsInChildren<Button>();
		int counter = 0;
		int selectedLev = 0;
		foreach(Button bu in PanelBUTTONS_Levels) {
			Debug.Log("gotSelectedForRanking : " + bu.gameObject.name);
			counter++;
			if(_val.Contains(counter.ToString() + "#")) {
				 selectedLev = counter;
				break;
			}
			
		}
		
		bool wantCurrentRanking = GameObject.Find("ToggleCurrentRanking").GetComponent<Toggle>().isOn;	
		string _scoreTable = "";
		
		
		
		Debug.Log("================= gotSelectedForRanking : " + _val + " : " + selectedGroup + " : " + selectedLev + " : " + wantCurrentRanking + " : " + _scoreTable);
		
		
		
		Debug.Log("gotSelectedForRanking : " + _val);
	  
	    labelLoading.enabled = true;
		
		string tableSelected = null;
		
		tableSelected = _scoreTable;
		
		showRanking(tableSelected);
	}

	public void gotButtonClick(GameObject _go) {

		if(_go.name == "BUTTON_SHOW_SELECTOR") {
			
			cleanRanKTable();
			
			int gameTypeCode = 0;
			int gameTypeDifficulty = 0;
			int gameTypeNormal = 0;
			
			for(int t = 0;t < gameTypeToggleList.Length;t++) { if(gameTypeToggleList[t].isOn) gameTypeCode = t;}
			 
			for(int t = 0;t < gameDifficultyToggleList.Length;t++) { if(gameDifficultyToggleList[t].isOn) gameTypeDifficulty = t;}
			
			for(int t = 0;t < gameNormalToggleList.Length;t++) { if(gameNormalToggleList[t].isOn) gameTypeNormal = t;}
			
			BBStaticVariable.setGameTypeAndDifficulty(gameTypeCode,gameTypeDifficulty,gameTypeNormal);
			
			string table = BBStaticVariable.getTablename(BBStaticVariable.currentlevelType,BBStaticVariable.currentLevelDifficulty,BBStaticVariable.currentLevelTypeNormal);
			
			Debug.Log("BUTTON_SHOW_SELECTOR : " + table);
			showRanking(table);
          
        }

		if(_go.name.Contains("_CLOSE")) {
		   SceneManager.LoadScene(0);
		}


   }


}
