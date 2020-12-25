using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanJew : Soldier{
    public Price price;

    SwordmanJew() : base(50f,1f, 10f,1f,1f,1f, 1.4f,3.08f){
        price.wood = 4f;
        price.rock = 0f;
        price.iron = 4f;
        price.wheat = 19f;
        price.water = 25f;
    }
}
