using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public Genre Genre;
    public ColorOfBook ColorOfBook;
    public int thickness;

    public bool placed;


    public Vector3 startPos;
    public Quaternion startRot;

    public void UpdatePositions()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }
}
