using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanJew : Soldier{
    public Price price;

    HorsemanJew() : base(130f,2f, 40f,2f,1.5f,1.5f, 4.2f,14f){
        price.wood = 21f;
        price.rock = 0f;
        price.iron = 15f;
        price.wheat = 55f;
        price.water = 65f;
    }
}
