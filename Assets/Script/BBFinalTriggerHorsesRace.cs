using UnityEngine;
using System.Collections;

public class BBFinalTriggerHorsesRace : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other) {
	
	  GameObject.FindGameObjectWithTag("GameController").SendMessage("gotHorseArrive",other.gameObject,SendMessageOptions.RequireReceiver);
	
	}
	
}
