using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

using UnityStandardAssets.CrossPlatformInput;

public class BBCustomPlayerController : MonoBehaviour {

   public GameObject[] toDeleteOnMobile;
  
	void Start () {
	
#if	MOBILE_INPUT
      foreach(GameObject go in toDeleteOnMobile) {
          go.SetActive(false);
      }
#endif
	}
	
}
