using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float sphereradius = 0.2f;
    [SerializeField] Color sphereColor = Color.red;

    private void OnDrawGizmos(){
        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(transform.position, sphereradius);
    }
}
