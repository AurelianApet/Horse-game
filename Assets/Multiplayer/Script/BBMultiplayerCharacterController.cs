using UnityEngine;
using System.Collections;

#if USE_PHOTON

public class BBMultiplayerCharacterController : MonoBehaviour {

	protected Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true;
	public float SynchronizedMaxSpeed;
	public float TurnSpeedModifier;
	public float SynchronizedTurnSpeed;
	public float SynchronizedSpeedAcceleration;
	
	protected PhotonView m_PhotonView;
	
	PhotonTransformView m_TransformView;
	
	//Vector3 m_LastPosition;
	float m_SpeedModifier;
	
	public bool canMove = false;
	
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		m_PhotonView = GetComponent<PhotonView>();
		m_TransformView = GetComponent<PhotonTransformView>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( m_PhotonView.isMine == false && PhotonNetwork.connected == true )
		{
			return;
		}
		
		if (animator && canMove)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
			
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButton("Fire1")) animator.SetBool("Jump", true);                
			}
			else
			{
				animator.SetBool("Jump", false);                
			}
			
			if(Input.GetButtonDown("Fire2") && animator.layerCount >= 2)
			{
				animator.SetBool("Hi", !animator.GetBool("Hi"));
			}
			
			
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			
			if( v < 0 )
			{
				v = 0;
			}
			
			animator.SetFloat( "Speed", h*h+v*v );
			animator.SetFloat( "Direction", h, DirectionDampTime, Time.deltaTime );
			
			float direction = animator.GetFloat( "Direction" );
			
			float targetSpeedModifier = Mathf.Abs( v );
			
			if( Mathf.Abs( direction ) > 0.2f )
			{
				targetSpeedModifier = TurnSpeedModifier;
			}
			
			m_SpeedModifier = Mathf.MoveTowards( m_SpeedModifier, targetSpeedModifier, Time.deltaTime * 25f );
			
			Vector3 speed = transform.forward * SynchronizedMaxSpeed * m_SpeedModifier;
			float turnSpeed = direction * SynchronizedTurnSpeed;
			
				
			m_TransformView.SetSynchronizedValues( speed, turnSpeed );
			
		}   		  
	}
	
	void gotHelloButton() {
	
		animator.SetBool("Hi", !animator.GetBool("Hi"));
	
	}
	
	
}
#endif