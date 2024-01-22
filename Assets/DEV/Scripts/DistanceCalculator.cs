using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    [SerializeField] public float shelfLength = 20f; 
    [SerializeField] public float[] bookThickness; 

    void Start()
    {
        calculateLength();
    }

    void calculateLength()
    {
        float kalanMesafe = shelfLength;

        
        foreach (float kalýnlýk in bookThickness)
        {
            kalanMesafe -= kalýnlýk;
        }

        
        Debug.Log("Rafta kalan mesafe: " + kalanMesafe + " cm");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("book"))
        {
            Debug.Log("temasvar");
        }
    }

}
