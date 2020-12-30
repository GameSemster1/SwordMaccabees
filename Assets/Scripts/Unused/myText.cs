using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myText : MonoBehaviour
{
 public string canvasText1 ="jj";
   void Start(){
        Invoke("DisableText", 5f); //invoke after 5 seconds
   }
   void DisableText(){ 
        canvasText1 = "";
   }    
}

