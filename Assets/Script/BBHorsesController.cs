using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBHorsesController : MonoBehaviour {

   public bool useMecAnim = false;
   public bool autoSetColors = false;
   public bool meshHaveSingleMaterial = false;
   public SkinnedMeshRenderer bodyMesh;
   public Color[] playerColorList;
	
   public int lineCode = 1;

    public float minSpeed = 5;
	public float maxSpeed = 7;
	float speed = 0;
    public AnimationClip[] idleAnim;
    
    Animation ani;
    Animator MecAni;
    
    bool canGo = false;
    
	public Vector3 horseStartingPos = Vector3.zero; 
	
	public bool isMultiplayer = false;
	public float[] multyRandData = new float[6];
	
	void Awake() {
	    
	    if(isMultiplayer) {
			gameObject.name = BBStaticVariable.getRunnerName(lineCode,BBStaticVariable.currentRunnerType);
		} else {
	        gameObject.name = BBStaticVariable.getRunnerName(lineCode,BBStaticVariable.currentRunnerType);
	    }
	}  
	  
    void changeSpeed() {
		speed = UnityEngine.Random.Range(minSpeed,maxSpeed);
    }

	void realStart() {
		GetComponent<AudioSource>().Play();
		
		if(!isMultiplayer) {
			speed = UnityEngine.Random.Range(minSpeed,maxSpeed);
		} else {
		}
		
		canGo = true;
		
		if(useMecAnim) {
			MecAni.SetBool("Run",true);
		} else {
			ani.CrossFade("Gallop");
		}
		
		if(!isMultiplayer) InvokeRepeating("changeSpeed",UnityEngine.Random.Range(2.0f,3.0f),UnityEngine.Random.Range(2.0f,3.0f));
		
    }
		
		
  public void realStart(float[] _speed) {
  
        GetComponent<AudioSource>().Play();
		
		 if(!isMultiplayer) {
		   speed = UnityEngine.Random.Range(minSpeed,maxSpeed);
		 } else {
		     switch(lineCode) { 
		       case 1: 
				if(_speed != null) speed = _speed[0];
		       break;
		       case 2: 
				if(_speed != null) speed = _speed[1];
		       break;
		       case 3: 
				if(_speed != null) speed = _speed[2];
		       break;
		       case 4: 
				if(_speed != null) speed = _speed[3];
		       break;
		       case 5: 
				if(_speed != null) speed = _speed[4];
		       break;
		       case 6: 
				if(_speed != null) speed = _speed[5];
		       break;
		      }
		 }
		
		canGo = true;
		
		if(useMecAnim) {
		   MecAni.SetBool("Run",true);
		} else {
		   ani.CrossFade("Gallop");
		}
		
		if(!isMultiplayer) InvokeRepeating("changeSpeed",UnityEngine.Random.Range(2.0f,3.0f),UnityEngine.Random.Range(2.0f,3.0f));
		
    }
    
	// Use this for initialization
	IEnumerator Start () {
	
	if(autoSetColors) setPlayerColor();
	 
	    horseStartingPos = transform.position;

	    if(useMecAnim) {
			MecAni = GetComponent<Animator>();
		} else {	 
	        ani = GetComponent<Animation>();
	    }
	    
	  
	  if(!isMultiplayer) {
	      yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f,0.05f));
	   }
	 
	  if(useMecAnim) {
	  
	  } else { 
		ani.clip =  idleAnim[UnityEngine.Random.Range(0,idleAnim.Length-1)];
	    ani.Play();
	  }
	   
	}
	
	
	void FixedUpdate() {
	   if(canGo) {
#if UNITY_WEBGL	   
			Vector3 wantedPos = Vector3.forward * speed * Time.deltaTime;
			
			if (!float.IsNaN(wantedPos.x) && !float.IsNaN(wantedPos.y) && !float.IsNaN(wantedPos.z))
			{
				transform.Translate(wantedPos);
			}
#else			
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
			
#endif			
		}
	}
	
 public	void stopRace() {
	   canGo = false;
		GetComponent<AudioSource>().Stop();
		
		if(useMecAnim) {
			MecAni.SetBool("Run",false);
		} else {
			ani.clip =  idleAnim[UnityEngine.Random.Range(0,idleAnim.Length-1)];
			ani.Play();
		}
		
		
	}
	
	void setPlayerColor() {
	
	  if(meshHaveSingleMaterial) {
		switch(lineCode) {
			case 1: bodyMesh.material.color = playerColorList[0]; break;
			case 2: bodyMesh.material.color = playerColorList[1]; break;
			case 3: bodyMesh.material.color = playerColorList[2]; break;
			case 4: bodyMesh.material.color = playerColorList[3]; break;
			case 5: bodyMesh.material.color = playerColorList[4]; break;
			case 6: bodyMesh.material.color = playerColorList[5]; break;
		}
	  } else {
	      switch(lineCode) {
		    case 1: bodyMesh.materials[1].color = playerColorList[0]; bodyMesh.materials[0].color = playerColorList[0]; break;
			case 2: bodyMesh.materials[1].color = playerColorList[1]; bodyMesh.materials[0].color = playerColorList[1]; break;
			case 3: bodyMesh.materials[1].color = playerColorList[2]; bodyMesh.materials[0].color = playerColorList[2]; break;
			case 4: bodyMesh.materials[1].color = playerColorList[3]; bodyMesh.materials[0].color = playerColorList[3]; break;
			case 5: bodyMesh.materials[1].color = playerColorList[4]; bodyMesh.materials[0].color = playerColorList[4]; break;
			case 6: bodyMesh.materials[1].color = playerColorList[5]; bodyMesh.materials[0].color = playerColorList[5]; break;
			
	      }
	    }
	}
	
}
