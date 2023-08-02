using UnityEngine;
using System.Collections;

public class BBStaticVariableMultiplayer  {



    public enum RunnerToExecuteInScene {robot,dude,horse,teddybig,none}
	public static RunnerToExecuteInScene runnerToExecuteInScene = RunnerToExecuteInScene.robot;

#if USE_PHOTON

#region multiplayerTmpData  	

    public static int myCurrentPositionOnTable = 0;

	public enum GameState {waitingForBet,raceOnGoing, waitingForNewRace};
	public static GameState gameState = GameState.waitingForBet;

	public static CloudRegionCode selectedRegionCode = CloudRegionCode.eu;
    public static string currentMPPlayerName = "";
	public static string currentMPRoomName = "";
	public static int currentMPmaxPlayerNumber = 2;
	public static string photonConnectionVersion = "10.5";
	
	
	public static bool gotMultiplayerMessage = false;
	public static string lastMultiplayerMessage = "";

		
	
#endregion	

	

#endif

}
