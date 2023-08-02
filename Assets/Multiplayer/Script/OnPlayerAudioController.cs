using UnityEngine;
using System.Collections;

public class OnPlayerAudioController : MonoBehaviour {

	private AudioSource GruntObstacoleHit;

	// Use this for initialization
	void Start () {
		GruntObstacoleHit = GameObject.Find("GruntObstacoleHit").GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision collision) {
	
		if(collision.collider.tag == "Obstacle") {
			if(!GruntObstacoleHit.isPlaying) 
				GruntObstacoleHit.Play();  
		}
		
	
	}
			
}
