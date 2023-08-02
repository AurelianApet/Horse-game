using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBCameraFollowMouse : MonoBehaviour {

	

	[System.Serializable]
	public class BBPlatformSettings{
	    public string deviceInfo;
		public RuntimePlatform platform;
		public string screenRatio;
		public float cameraSizeDefault;
		public float cameraSizeZoomed;
        public bool useZoom;	
		public bool useCameraFollowTouch;	
	}
	public List<BBPlatformSettings> _BBPlatformSettings;
 
	public float speed = 0.5F;
		
	bool canDrag = true;
	
	float currentCameraSizeDefault = 0;
	float currentCameraSizeZoomed = 0;
	Vector3 startingCameraPosition = Vector3.zero;
	
	void Awake() {
	 startingCameraPosition = transform.position;
	 canDrag = false;
	 
		if(!cameraPivotController) cameraPivotController = GameObject.Find("cameraPivotController");
	}
	
	void Start () 
	{
	 string camRatio = "3:2";
	 
	 
		if (Camera.main.aspect >= 1.7){ camRatio = "16:9"; Debug.Log(camRatio);} // altri
		else if (Camera.main.aspect >= 1.5){ camRatio = "3:2"; Debug.Log(camRatio);} // iphone 4
		else {camRatio = "4:3"; Debug.Log(camRatio);} // ipad
	
	   for(int x = 0; x < _BBPlatformSettings.Count; x++) {
	   
#if UNITY_IOS	   
			if(_BBPlatformSettings[x].platform == RuntimePlatform.IPhonePlayer || Application.platform  == RuntimePlatform.OSXEditor) {
	            if(camRatio ==  _BBPlatformSettings[x].screenRatio) {
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoom").SetActive(false);
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoomMoveing").SetActive(false);
					if(!_BBPlatformSettings[x].useCameraFollowTouch) canDrag = false;
					GetComponent<Camera>().orthographicSize = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeDefault = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeZoomed = _BBPlatformSettings[x].cameraSizeZoomed;
	            }
	        }
#endif
#if UNITY_WEBGL
			if(_BBPlatformSettings[x].platform == RuntimePlatform.WebGLPlayer || Application.platform  == RuntimePlatform.OSXEditor) {
				if(camRatio ==  _BBPlatformSettings[x].screenRatio) {
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoom").SetActive(false);
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoomMoveing").SetActive(false);
					if(!_BBPlatformSettings[x].useCameraFollowTouch) canDrag = false;
					GetComponent<Camera>().orthographicSize = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeDefault = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeZoomed = _BBPlatformSettings[x].cameraSizeZoomed;
				}
			}
#endif			
#if UNITY_STANDALONE
			if(_BBPlatformSettings[x].platform == RuntimePlatform.OSXPlayer || _BBPlatformSettings[x].platform == RuntimePlatform.WindowsPlayer) {
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoom").SetActive(false);
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoomMoveing").SetActive(false);
					if(!_BBPlatformSettings[x].useCameraFollowTouch) canDrag = false;
					GetComponent<Camera>().orthographicSize = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeDefault = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeZoomed = _BBPlatformSettings[x].cameraSizeZoomed;
				
			}
#endif	
#if UNITY_ANDROID
			if(_BBPlatformSettings[x].platform == RuntimePlatform.Android || Application.platform  == RuntimePlatform.OSXEditor) {
				if(camRatio ==  _BBPlatformSettings[x].screenRatio) {
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoom").SetActive(false);
					if(!_BBPlatformSettings[x].useZoom) GameObject.Find("ButtonRouletteZoomMoveing").SetActive(false);
					if(!_BBPlatformSettings[x].useCameraFollowTouch) canDrag = false;
					GetComponent<Camera>().orthographicSize = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeDefault = _BBPlatformSettings[x].cameraSizeDefault;
					currentCameraSizeZoomed = _BBPlatformSettings[x].cameraSizeZoomed;
				}
			}
			#endif				
			
	   }
	
	
	}
	
	void gotZoomButton(GameObject _go) {
	  
     if( GetComponent<Camera>().orthographicSize == currentCameraSizeDefault) {
			GetComponent<Camera>().orthographicSize = currentCameraSizeZoomed;
			canDrag = true;
      } else {
			GetComponent<Camera>().orthographicSize = currentCameraSizeDefault;
			canDrag = false;
			transform.position = startingCameraPosition;
      } 	     
	  
	}
	
	void gotDrag(bool stopDrag) {
	}			
	
	public GameObject cameraPivotController; 
	Vector3 lastGoodPos = Vector3.zero;
	void Update()
	{
	
	
		if(!canDrag) return;
	
	     
			
		float dist = Vector3.Distance(cameraPivotController.transform.position, transform.position);
	//	print("Distance to other: " + dist);
		
		if(dist > 30) { 
			   transform.position = lastGoodPos; 
		       return;
         }
        lastGoodPos = transform.position;
      
 /*           
		Vector3 screenResolution = new Vector3(Screen.width, Screen.height, 0.0f); 
		Vector3 mousePosition = Input.mousePosition; 
		
		mousePosition.y = Screen.height - mousePosition.y; 
		mousePosition.x = Screen.width - mousePosition.x; 
		screenResolution *= 0.5f; 
		Vector3 difference = mousePosition - screenResolution; 
		transform.Translate(difference * Time.smoothDeltaTime * -0.05f); 
		//transform.LookAt(m_Target.transform); 
		Vector3 newPosition = transform.position; 
		//newPosition.z = 50f; 
		transform.position = newPosition; 
				
*/
			if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				transform.Translate(-touchDeltaPosition.x * speed * Time.deltaTime, -touchDeltaPosition.y * speed * Time.deltaTime, 0);
			}
			
	}	
	
}
