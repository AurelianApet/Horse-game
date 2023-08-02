using UnityEngine;
using System.Collections;

public class BBStartControllerMultiplayer : MonoBehaviour {

    public GameObject dudeRunnersPrefab;
	public GameObject robotRunnersPrefab;
	public GameObject horseRunnersPrefab;
	public GameObject teddybigRunnersPrefab;
	

   void Awake() {
#if USE_PHOTON
		Debug.Log("[MPInSceneController][Start] ***runnerstype*** : " + (int)PhotonNetwork.room.customProperties["runnerstype"]);
		BBStaticVariableMultiplayer.runnerToExecuteInScene = (BBStaticVariableMultiplayer.RunnerToExecuteInScene)PhotonNetwork.room.customProperties["runnerstype"];
#endif		
      
      switch(BBStaticVariableMultiplayer.runnerToExecuteInScene) {
			case BBStaticVariableMultiplayer.RunnerToExecuteInScene.dude: Instantiate(dudeRunnersPrefab); break;
		    case BBStaticVariableMultiplayer.RunnerToExecuteInScene.robot: Instantiate(robotRunnersPrefab); break;
		    case BBStaticVariableMultiplayer.RunnerToExecuteInScene.horse: Instantiate(horseRunnersPrefab); break;
		    case BBStaticVariableMultiplayer.RunnerToExecuteInScene.teddybig: Instantiate(teddybigRunnersPrefab); break;
      }
   
   }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
