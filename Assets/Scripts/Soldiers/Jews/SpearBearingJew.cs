using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBreatingJew : Soldier{
    public Price price;

    SpearBreatingJew() : base(50f,1.2f, 15f,2f,1.5f,1.2f, 1.2f,2.7f){
        price.wood = 6f;
        price.rock = 0f;
        price.iron = 4f;
        price.wheat = 20f;
        price.water = 26f;
    }
}
