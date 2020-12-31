using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    [SerializeField] Canvas textToDisapeare;
    [SerializeField] Canvas textToApeare;   

    private int time = 0; 

    void Stard(){
        textToApeare.enabled = false;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && time == 0){
            textToDisapeare.enabled = false;
            textToApeare.enabled = true;
            time = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && time == 1){
            Destroy(this.gameObject);
        }
    }
}
