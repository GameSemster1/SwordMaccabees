using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerJew : Worker{
    public Price price;

    WorkerJew() :
    base(30f,1f, 5f,1f,1.5f,1f, 1.12f,2.8f, 3.5f,1f,10f){
       price.wood = price.rock = price.iron = 0f;
       price.wheat = 14;
       price.water = 10f;
    }

}