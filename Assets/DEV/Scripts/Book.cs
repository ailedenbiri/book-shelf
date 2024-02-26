using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public Genre Genre;
    public ColorOfBook ColorOfBook;
    public int thickness;

    [HideInInspector] public bool placed;


    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Quaternion startRot;

    private void Start()
    {
        GameAssets.i.AddSpriteToBook(this);
    }

    public void UpdatePositions()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }
}
