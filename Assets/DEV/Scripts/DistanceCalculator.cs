using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    public Vector3 AddPositionCalculate(Vector3 currentPos, float beforeObjectSize, float afterObjectSize)
    {
        currentPos.x+= (beforeObjectSize+afterObjectSize)/2;

        return currentPos;

    }

    public Vector3 OofPositionCalculate(Vector3 currentPos, float beforeObjectSize, float afterObjectSize)
    {
        currentPos.x -= (beforeObjectSize + afterObjectSize)/2;

        return currentPos;

    }


}
