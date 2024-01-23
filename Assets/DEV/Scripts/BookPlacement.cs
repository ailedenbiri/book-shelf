using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPlacement : MonoBehaviour
{
    

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            DistanceCalculator.instance.AddToLength(3);
        }

    }
}
