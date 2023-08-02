
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	
	public class BBCustomThirdPersonCharacter : MonoBehaviour
	{
		public bool isMultiplayer = false;
		bool isHurdlesRace = false;
		
		public enum PlayerAnimState {idle, stand , walk, run}; 
		public PlayerAnimState playerAnimState;
		
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 12f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;
		
		Rigidbody m_Rigidbody;
		[HideInInspector]
		public Animator m_Animator;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;
		
		
		public AudioSource breatheRunnigAudio;
		[Header("0 = idle 1 = walk 2 = run 3 volume")]
		public Vector4 breathePichData = new Vector4(08f,0.9f,1,0.5f); 
		public float defaultVolumeVal = 0.5f;
		
		private Text TextCurrentSpeedMS;
		[HideInInspector]
		public float CurrentSpeed = 0;
		private float starting_AnimSpeedMultiplier = 0;
		
		public bool isGhostPlayer = false;
		
		public bool wantDecreaseSpeed = false;
		
		public float maximumSpeedAllowed = 1.3f;
		
		private Slider SliderPlayerPower;
		
		public bool isThePlayerInARaceScene = true;
		
		private Text TextPlayerSpeed;
		
		void Awake()
		{
		
		if(SceneManager.GetActiveScene().name.Contains("_O")) {
		   isHurdlesRace = true;
		} else {
			Destroy(GameObject.Find("JumpButton"));
		}
		
		
		
		if(isMultiplayer) {
				//breatheRunnigAudio = transform.FindChild("SoundController/breatheRunnigAudio").GetComponent<AudioSource>();
				
				starting_AnimSpeedMultiplier = m_AnimSpeedMultiplier;
				
				m_Animator = GetComponent<Animator>();
				m_Rigidbody = GetComponent<Rigidbody>();
				m_Capsule = GetComponent<CapsuleCollider>();
				m_CapsuleHeight = m_Capsule.height;
				m_CapsuleCenter = m_Capsule.center;
				
				m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
				m_OrigGroundCheckDistance = m_GroundCheckDistance;
				
				SliderPlayerPower = GameObject.Find("SliderPlayerPower").GetComponent<Slider>();
				SliderPlayerPower.maxValue = maximumSpeedAllowed;
				InvokeRepeating("decreaseSpeed",1,1);
				
				TextCurrentSpeedMS = GameObject.Find("TextCurrentSpeedMS").GetComponent<Text>();
				
		  return;
		}
	
		 if(isThePlayerInARaceScene) {
		 
			 if(!isGhostPlayer)	{ 
			    TextPlayerSpeed = transform.FindChild("playerInfoHud/TextPlayerSpeed").gameObject.GetComponent<Text>();
				
				GameObject playerInfoHudRoot = transform.FindChild("playerInfoHud").gameObject;
				
				playerInfoHudRoot.transform.FindChild("RawImageCountry").GetComponent<RawImage>().texture = BBStaticVariable.getTextureByCountryCode(PlayerPrefs.GetString("countryCode"));
				playerInfoHudRoot.transform.FindChild("TextPlayerName").GetComponent<Text>().text = PlayerPrefs.GetString("PlayerNickName");
				
		 	
				SliderPlayerPower = GameObject.Find("SliderPlayerPower").GetComponent<Slider>();
				SliderPlayerPower.maxValue = maximumSpeedAllowed;
			
				InvokeRepeating("decreaseSpeed",1,1);
			
				starting_AnimSpeedMultiplier = m_AnimSpeedMultiplier;
			   
				TextCurrentSpeedMS = GameObject.Find("TextCurrentSpeedMS").GetComponent<Text>();
				
				breatheRunnigAudio = GameObject.Find("breatheRunnigAudio").GetComponent<AudioSource>();
			
			}
			
		} else {
				GameObject playerInfoHudRoot = transform.FindChild("playerInfoHud").gameObject;
				playerInfoHudRoot.SetActive(false);
		}
		
		
			breatheRunnigAudio = GameObject.Find("breatheRunnigAudio").GetComponent<AudioSource>();
			
			starting_AnimSpeedMultiplier = m_AnimSpeedMultiplier;
			
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;
			
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
		}
		
		
		
		public void setRecortedData(Quaternion _data) {
			m_Animator.SetFloat("Forward",_data[0]);
			m_Animator.SetFloat("Turn",_data[1]);
			m_Animator.SetBool("OnGround", (_data[2] == 1) ? true : false );
			m_Animator.SetFloat("Jump",_data[3]);
			
		}
		
		public void Move(Vector3 move, bool crouch, bool jump)
		{
		
		
			
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			
			ApplyExtraTurnRotation();
			
			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(crouch, jump);
			}
			else
			{
				HandleAirborneMovement();
			}
			
		//	ScaleCapsuleForCrouching(crouch);
		//	PreventStandingInLowHeadroom();
			
			// send input and other state parameters to the animator
			UpdateAnimator(move);
			
			setBreatheAudio();
			
			setSpeed();
			
			if(Input.GetKeyDown(KeyCode.LeftShift)) {
				increasingSpeed();
			}
			
			
			if(isThePlayerInARaceScene) SliderPlayerPower.value = m_MoveSpeedMultiplier;
			
		}
		
		void setSpeed() {
		
			CurrentSpeed = m_Rigidbody.velocity.magnitude;
		
		if(isMultiplayer) { 
				TextCurrentSpeedMS.text = "KPH : " + (CurrentSpeed * 3.6f).ToString("F2");
				//TextPlayerSpeed.text = CurrentSpeed.ToString("F2");
				
		     return;
		
		}
		   if(isThePlayerInARaceScene) { 
		      TextCurrentSpeedMS.text = "KPH : " + (CurrentSpeed * 3.6f).ToString("F2");
			  TextPlayerSpeed.text = CurrentSpeed.ToString("F2");
			}
		}
		
		
		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}
		
		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength))
				{
					m_Crouching = true;
				}
			}
		}
		
		
		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}
			
			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}
			
			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_AnimSpeedMultiplier = m_MoveSpeedMultiplier;
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
			
			if (m_IsGrounded && move.magnitude == 0)
			{
			  m_MoveSpeedMultiplier = starting_AnimSpeedMultiplier;
			}
		}
		
		
		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);
			
			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}
		
		
		void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
			   if(isHurdlesRace) {
					// jump!
					m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
					m_IsGrounded = false;
					m_Animator.applyRootMotion = false;
					m_GroundCheckDistance = 0.1f;
			   }
			}
		}
		
		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}
		
		
		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
				
				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}
		
		void decreaseVolume() {
			if(breathePichData[3] > 0)
				breathePichData[3] -= 0.02f;
			else CancelInvoke("decreaseVolume");
		}
		
		void setBreatheAudio() {
			
			AnimatorClipInfo[] ac = m_Animator.GetCurrentAnimatorClipInfo(0);
			
			foreach(AnimatorClipInfo _ac in ac) { 
				if(_ac.clip.name.Contains("Run")) playerAnimState = PlayerAnimState.run;
				else if(_ac.clip.name.Contains("Walk")) playerAnimState = PlayerAnimState.walk;
				else if(_ac.clip.name.Contains("Stand")) playerAnimState = PlayerAnimState.stand;
				else if(_ac.clip.name.Contains("Idle")) playerAnimState = PlayerAnimState.idle;
				//  else RPC(_ac.clip.name);
				
			}
			
			switch(playerAnimState) {
			case PlayerAnimState.walk: breatheRunnigAudio.pitch = breathePichData[1];  
				breathePichData[3] = defaultVolumeVal;
				CancelInvoke("decreaseVolume");
				break;
			case PlayerAnimState.idle: 
			case PlayerAnimState.stand: 
				if(!IsInvoking("decreaseVolume")) InvokeRepeating("decreaseVolume",0.2f,0.2f);
				if(breatheRunnigAudio) breatheRunnigAudio.pitch = breathePichData[0];  break;
				
			case PlayerAnimState.run: 
				breatheRunnigAudio.pitch = breathePichData[2];  
				breathePichData[3] = defaultVolumeVal;
				CancelInvoke("decreaseVolume");
				
				break;
			default :
				
				break;	
				
			}
			
			breatheRunnigAudio.volume = breathePichData[3];
			
			// RPC(playerAnimState); 
		}
		
		
		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
			#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
			#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}
		
		void gotObstacleCollsion() {
//			RPC("[BBCustomThirdPersonCharacter]gotObstacleCollsion");
			if(!IsInvoking("decreaseSpeedAfterCollision")) {
				 InvokeRepeating("decreaseSpeedAfterCollision",0.5f,0.5f);
			}
		
		}
	
		float tempSpeedMultiplierCounter = 0;
		bool inDecreaseingSpeed = false;
		float lastSpeedMultiplier = 0;
		float stopDecreaseingAtCounter = 5;
		 	
		void decreaseSpeedAfterCollision() {
		
		   if(!inDecreaseingSpeed) {
				inDecreaseingSpeed = true;
				lastSpeedMultiplier = m_MoveSpeedMultiplier;
		   } else {
				tempSpeedMultiplierCounter ++;
				m_MoveSpeedMultiplier = 0.9f; 
				
				if(tempSpeedMultiplierCounter > stopDecreaseingAtCounter) {
				  CancelInvoke("decreaseSpeedAfterCollision");
				  m_MoveSpeedMultiplier = lastSpeedMultiplier;
				  inDecreaseingSpeed = false;
					tempSpeedMultiplierCounter = 0;
				}
				
		   }
		
//			RPC(m_MoveSpeedMultiplier + " : " + tempSpeedMultiplierCounter);
			
			
		      
		}
		
		void increasingSpeed() {
			
			if(m_MoveSpeedMultiplier > maximumSpeedAllowed) {
				m_MoveSpeedMultiplier = maximumSpeedAllowed;
			} else {
				m_MoveSpeedMultiplier += 0.01f;
			}
			
		}
		
		void decreaseSpeed() {
		
		  if(m_MoveSpeedMultiplier < 1.1f) {
				m_MoveSpeedMultiplier = 1.1f;
		  } else {
				m_MoveSpeedMultiplier -= 0.01f;
		  }
		
		}
		
		
	}
	
}

