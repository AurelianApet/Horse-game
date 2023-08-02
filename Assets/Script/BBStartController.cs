using UnityEngine;
using System.Collections;

public class BBStartController : MonoBehaviour {

	public GameObject dudeRunnersPrefab;
	public GameObject robotRunnersPrefab;
	public GameObject horseRunnersPrefab;
	public GameObject teddybigRunnersPrefab;
	
	public bool isMultiplayerScene = true;
	
	void Awake() {

#if USE_PHOTON		
	
		if(isMultiplayerScene) { 	
			switch(BBStaticVariableMultiplayer.runnerToExecuteInScene) {
			case BBStaticVariableMultiplayer.RunnerToExecuteInScene.dude: Instantiate(dudeRunnersPrefab); break;
			case BBStaticVariableMultiplayer.RunnerToExecuteInScene.robot: Instantiate(robotRunnersPrefab); break;
			case BBStaticVariableMultiplayer.RunnerToExecuteInScene.horse: Instantiate(horseRunnersPrefab); break;
			case BBStaticVariableMultiplayer.RunnerToExecuteInScene.teddybig: Instantiate(teddybigRunnersPrefab); break;
			}
		} else {
			switch(BBStaticVariable.runnerToExecuteInScene) {
			case BBStaticVariable.RunnerToExecuteInScene.dude: Instantiate(dudeRunnersPrefab); break;
			case BBStaticVariable.RunnerToExecuteInScene.robot: Instantiate(robotRunnersPrefab); break;
			case BBStaticVariable.RunnerToExecuteInScene.horse: Instantiate(horseRunnersPrefab); break;
			case BBStaticVariable.RunnerToExecuteInScene.teddybig: Instantiate(teddybigRunnersPrefab); break;
			}
		}
		
#else
		switch(BBStaticVariable.runnerToExecuteInScene) {
		case BBStaticVariable.RunnerToExecuteInScene.dude: Instantiate(dudeRunnersPrefab); break;
		case BBStaticVariable.RunnerToExecuteInScene.robot: Instantiate(robotRunnersPrefab); break;
		case BBStaticVariable.RunnerToExecuteInScene.horse: Instantiate(horseRunnersPrefab); break;
		case BBStaticVariable.RunnerToExecuteInScene.teddybig: Instantiate(teddybigRunnersPrefab); break;
		}
#endif
		
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	

}
