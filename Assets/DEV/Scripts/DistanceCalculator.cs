using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    public static DistanceCalculator instance;
    [SerializeField] public int shelfLength = 10;

    private void Awake()
    {
        instance = this;
    }
    public void AddToLength(int bookThickness)
    {

        if(shelfLength-bookThickness>0)
        {
            shelfLength -= bookThickness;
            Debug.Log("Remaining shelf length: " + shelfLength);
        }
        else if (shelfLength - bookThickness == 0) { Debug.Log("Shelf fit perfectly"); }
        else { Debug.Log("Doesn't fit"); }
    }

    public void OofTheShelf(int bookThickness)
    {
        shelfLength += bookThickness;
        Debug.Log("Remaining shelf length: " + shelfLength);
    }


}
