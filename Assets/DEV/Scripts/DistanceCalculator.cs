using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DistanceCalculator : MonoBehaviour
{
    public static DistanceCalculator instance;
    [SerializeField] public int shelfLength = 10;
    [SerializeField] public BookSettingsScriptableObject bookSettings;
    [SerializeField] public Vector3 currentPos;
    List<Book> addedBooks = new List<Book>();
    [SerializeField] public float sizeCoefficient;
    private bool added;
    private void Awake()
    {
        instance = this;
        added = false;
    }
    public void AddToLength(int bookThickness)
    {

        if(shelfLength-bookThickness>0)
        {
            shelfLength -= bookThickness;
            Debug.Log("Remaining shelf length: " + shelfLength);
            added = true;
        }
        else if (shelfLength - bookThickness == 0) 
        {
            added = true; 
            Debug.Log("Shelf fit perfectly"); 
        }
        else
        {
            added = false;
            Debug.Log("Doesn't fit"); 
        }
    }


    public Vector3 AddPositionCalculate(Book book)
    {
        if (addedBooks.Count == 0 && book.Genre == bookSettings.Genre && book.ColorOfBook == bookSettings.ColorOfBook)
        {
            AddToLength(book.thickness);
            Debug.Log("Again same position");
            if (added == true)
            {
                addedBooks[addedBooks.Count - 1] = book;
                added = false;
                
            }
            return currentPos; 
            
        }
        else if(addedBooks.Count !=0  && book.Genre == bookSettings.Genre && book.ColorOfBook == bookSettings.ColorOfBook) 
        {
            
            AddToLength(book.thickness);
            if(added == true)
            {
                currentPos.x += (addedBooks[addedBooks.Count - 1].thickness * sizeCoefficient + book.thickness * sizeCoefficient) / 2;
                addedBooks[addedBooks.Count - 1] = book;
                Debug.Log("Added new position");
                added = false;
                
            }
            return currentPos; 
        }
        return currentPos;

    }

}
