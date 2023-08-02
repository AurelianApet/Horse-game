using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBDrawContainerControl : MonoBehaviour {

  public Image imageHideContainer;
  public Sprite spriteHide;
  public Sprite spriteView;
  public GameObject containerToHide;
	
	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void gotHideShowContailnerButton(GameObject _go) {
	
		if(imageHideContainer.sprite.name == "Windowed") {
			imageHideContainer.sprite = spriteView;
			containerToHide.SetActive(false);
	   } else {
			imageHideContainer.sprite = spriteHide;
			containerToHide.SetActive(true);
	   }
	
	   
	
	}
}
