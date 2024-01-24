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

    private void Start()
    {
        startPos = transform.position;
    }
}
