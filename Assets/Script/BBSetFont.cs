using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBSetFont : MonoBehaviour {

	public Font _font;

 void Start() {
 
	Text[] allText = GetComponentsInChildren<Text>(true);
	
	foreach(Text t in allText) {
		if(!t.gameObject.name.Contains("INPUT")) t.font = _font;
	}
	
  }
  	
}
