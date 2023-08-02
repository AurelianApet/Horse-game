using UnityEngine;
using System.Collections;


public class BBChipData : MonoBehaviour {

   public TextMesh TextChipValueMulti;
	
   public Material[] multiplayerMatList;
  
   public int posID;
   public float betValue;
   public string s_posID;	
   public bool canRemoveChip = true;
		
   public Material greenMat;
   public Material redMat;
   public Material blueMat;
   public Material doubleMat;
   
	
  public MeshRenderer meshRend;	

  public GameObject resultCanvas; 
  
  public int relatedPlayerPositition = 0;
}
