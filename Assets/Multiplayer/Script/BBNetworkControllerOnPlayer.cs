using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBNetworkControllerOnPlayer : MonoBehaviour {

 public MonoBehaviour[] MonoBToDeleteOnNotMine;
 public Rigidbody compRigidbody;
 
 private Animator _animator;

#if USE_PHOTON

  
   
	public PhotonView pv;

	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	
	// Use this for initialization
	void Awake () {
	 
	   _animator = GetComponent<Animator>();
	   string countryCode = "";
	   
	   if(!pv.isMine) {
	   
	     foreach(MonoBehaviour mb in MonoBToDeleteOnNotMine) mb.enabled = false;
	     compRigidbody.useGravity = false;
	     compRigidbody.isKinematic = true;
		 gameObject.tag = "Remote";
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- > ### NOT isMine NOT ### : " + pv.owner.name);
			countryCode =  (string)pv.owner.customProperties["pcountry"];
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- > ### NOT isMine NOT ### COUNTRY CODE: " + countryCode + " avatarCode : " + (string)pv.owner.customProperties["avatarCode"]);
			gameObject.name = pv.owner.name;
	   
	   } else {
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- > ### YES isMine YES ### : " + PhotonNetwork.player.name);
			countryCode =  (string)PhotonNetwork.player.customProperties["pcountry"];
			gameObject.name = PhotonNetwork.player.name;
			BBStaticVariable.BBLog("BBNetworkControllerOnPlayer --> Awake -- >  ### YES isMine YES ### COUNTRY CODE: " + countryCode + " avatarCode : " + (string)pv.owner.customProperties["avatarCode"]);
			
	   }
	   
	
		
		//_BBRecordingController.worldBestPlayerStruct[x].playerInfoHud = _BBRecordingController.worldBestPlayerStruct[x].player.transform.FindChild("playerInfoHud").gameObject;
		//_BBRecordingController.worldBestPlayerStruct[x].playerInfoHud.transform.FindChild("RawImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(_BBRecordingController.worldBestPlayerStruct[x].worldBestCountry); 
		//_BBRecordingController.worldBestPlayerStruct[x].playerInfoHud.transform.FindChild("TextPlayerName").GetComponent<Text>().text = _BBRecordingController.worldBestPlayerStruct[x].worldBestNick; 
	
		transform.FindChild("playerInfoHud/RawImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(countryCode); 
		transform.FindChild("playerInfoHud/TextPlayerName").GetComponent<Text>().text = gameObject.name; 
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!pv.isMine)
		{
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
		}
		
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			Quaternion data = new Quaternion(_animator.GetFloat("Forward"),_animator.GetFloat("Turn"), (_animator.GetBool("OnGround") == true) ? 1 : 0,_animator.GetFloat("Jump"));
			stream.SendNext((Quaternion)data);
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation); 
		}
		else
		{
			setRecortedData((Quaternion) stream.ReceiveNext());
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}
	
	public void setRecortedData(Quaternion _data) {
		_animator.SetFloat("Forward",_data[0]);
		_animator.SetFloat("Turn",_data[1]);
		_animator.SetBool("OnGround", (_data[2] == 1) ? true : false );
		_animator.SetFloat("Jump",_data[3]);
	}
	
#endif
		
}
