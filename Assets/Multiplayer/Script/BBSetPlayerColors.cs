using UnityEngine;
using System.Collections;

public class BBSetPlayerColors : MonoBehaviour {

	public SkinnedMeshRenderer m_hair;
	public SkinnedMeshRenderer _Eyes;
	public SkinnedMeshRenderer PlayerBody;
	
	
	void Start() {
		
	}
	
	public void setColor(Color[] colList) {
		
		PlayerBody.material.color = colList[0];
		m_hair.material.color = colList[1];
		_Eyes.material.color = colList[2];
	}
	
	
	void loadCleanMaterial() {
	}
	
	
}
