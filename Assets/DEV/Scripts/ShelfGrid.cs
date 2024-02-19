using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfGrid : MonoBehaviour
{
    public Book book;
    public ColorOfBook color = ColorOfBook.Empty;
    public Genre genre = Genre.ChildBook;
    public DistanceCalculator shelf;

    public bool isEmpty = true;

    public void UpdateColor()
    {
       //  if (color != ColorOfBook.Empty) transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = GameAssets.i.bookColors[(int)this.color];
    }
}
