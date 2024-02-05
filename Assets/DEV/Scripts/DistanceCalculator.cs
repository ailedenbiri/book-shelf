using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        currentPos = transform.GetChild(0).position;
    }
    public void AddToLength(int bookThickness)
    {
        GameManager.instance.CountBooks();
        if (shelfLength - bookThickness > 0)
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
        
        if (addedBooks.Count == 0 && book.Genre == bookSettings.Genre && (book.ColorOfBook == bookSettings.ColorOfBook || bookSettings.ColorOfBook == ColorOfBook.Empty))
        {
            AddToLength(book.thickness);
            currentPos.x += book.thickness * 0.05f;
            Debug.Log("Again same position");
            DOVirtual.DelayedCall(1f, () => { GameManager.instance.UnlockBooks(); });
            if (added == true)
            {
                addedBooks.Add(book);
                added = false;
            }
            BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
            DOVirtual.DelayedCall(1f, () => GameManager.instance.state = GameManager.GameState.Playing);
            book.placed = true;
            GameManager.instance.CountBooks();
            return currentPos;

        }
        else if (addedBooks.Count != 0 && book.Genre == bookSettings.Genre && (book.ColorOfBook == bookSettings.ColorOfBook || bookSettings.ColorOfBook == ColorOfBook.Empty))
        {

            AddToLength(book.thickness);
            DOVirtual.DelayedCall(1f, () => { GameManager.instance.UnlockBooks(); });

            if (added == true)
            {
                currentPos.x += (addedBooks[addedBooks.Count - 1].thickness * sizeCoefficient + book.thickness * sizeCoefficient) / 2;
                addedBooks.Add(book);
                Debug.Log("Added new position");
                added = false;

            }
            BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
            DOVirtual.DelayedCall(1f, () => GameManager.instance.state = GameManager.GameState.Playing);
            book.placed = true;
            GameManager.instance.CountBooks();
            return currentPos;
        }
        else
        {
            if (addedBooks.Count == 0)
            {
                Vector3 tempPos = currentPos;
                currentPos.x += book.thickness * 0.05f;
                Transform tempBook = book.transform;
                BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
                currentPos = tempPos;
                DOVirtual.DelayedCall(2f, () =>
                {
                    tempBook.DOKill();
                    BookController.instance.ReturnToOriginalPosition(tempBook, true);
                    tempBook.GetComponent<Book>().placed = false;
                    tempBook.GetComponent<Collider>().enabled = true;
                    GameManager.instance.Vibrate();
                    
                });
                GameManager.instance.WrongShelf();
                return currentPos;
            }
            else
            {
                //Wrong shelf!!!
                Debug.Log("Wrong Shelf!!");
                currentPos.x += (addedBooks[addedBooks.Count - 1].thickness * sizeCoefficient + book.thickness * sizeCoefficient) / 2;
                Transform tempBook = book.transform;
                BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
                DOVirtual.DelayedCall(2f, () =>
                {
                    tempBook.DOKill();
                    BookController.instance.ReturnToOriginalPosition(tempBook, true);
                    tempBook.GetComponent<Book>().placed = false;
                    tempBook.GetComponent<Collider>().enabled = true;
                    GameManager.instance.Vibrate();
                    WrongShelfPosCalculate(tempBook.GetComponent<Book>());
                });
                GameManager.instance.WrongShelf();
                return currentPos;
            }

        }

    }
    public void WrongShelfPosCalculate(Book book)
    {
        currentPos.x -= (addedBooks[addedBooks.Count - 1].thickness * sizeCoefficient + book.thickness * sizeCoefficient) / 2;


    }


}
