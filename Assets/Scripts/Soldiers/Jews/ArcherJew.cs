using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJew : Soldier{
    public Price price;

    ArcherJew() : base(50f,0.8f, 25f,30f,3f,1.7f, 0f,3.64f){
        price.wood = 15f;
        price.rock = 0f;
        price.iron = 8f;
        price.wheat = 15f;
        price.water = 20f;
    }
    
}
