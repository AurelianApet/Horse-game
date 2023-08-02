using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBMainMenuControlMultiplayer : MonoBehaviour {

	public GameObject BaseMenuWindow;
	public GameObject multiplayerWindows;
	public GameObject multiplayerConnectController;
	

	// Use this for initialization
	void Start () {
    

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public void gotButtonClick() {
//		Application.LoadLevel("demoMPConnect");
//	}
	
	public void buttonsClickController(GameObject _go) {
	
	    switch(_go.name) {
			case "BUTTON_PLAY_MULTIPLAYER" :
				BaseMenuWindow.SetActive(false);
				multiplayerWindows.SetActive(true);
				multiplayerConnectController.SetActive(true);
			break;
	    }
	
	}
	
	
}
