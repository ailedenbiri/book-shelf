using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
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
        if (addedBooks.Count == 0 && book.Genre == bookSettings.Genre && book.ColorOfBook == bookSettings.ColorOfBook)
        {
            AddToLength(book.thickness);
            Debug.Log("Again same position");
            DOVirtual.DelayedCall(1f, () => { GameManager.instance.UnlockBooks(); });
            if (added == true)
            {
                addedBooks.Add(book);
                added = false;
            }
            BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
            DOVirtual.DelayedCall(1f, () => GameManager.instance.state = GameManager.GameState.Playing);
            return currentPos;

        }
        else if (addedBooks.Count != 0 && book.Genre == bookSettings.Genre && book.ColorOfBook == bookSettings.ColorOfBook)
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
            return currentPos;
        }
        else
        {
            if (addedBooks.Count == 0)
            {
                Transform tempBook = book.transform;
                BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    tempBook.DOKill();
                    BookController.instance.ReturnToOriginalPosition(tempBook, true);
                    tempBook.GetComponent<Book>().placed = false;
                    tempBook.GetComponent<Collider>().enabled = true;
                    GameManager.instance.Vibrate();
                });
                return currentPos;
            }
            else
            {
                //Wrong shelf!!!
                Debug.Log("Wrong Shelf!!");
                currentPos.x += (addedBooks[addedBooks.Count - 1].thickness * sizeCoefficient + book.thickness * sizeCoefficient) / 2;
                Transform tempBook = book.transform;
                BookController.instance.PlaceBookOnShelf(book.transform, currentPos);
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    tempBook.DOKill();
                    BookController.instance.ReturnToOriginalPosition(tempBook, true);
                    tempBook.GetComponent<Book>().placed = false;
                    tempBook.GetComponent<Collider>().enabled = true;
                    GameManager.instance.Vibrate();
                    WrongShelfPosCalculate(tempBook.GetComponent<Book>());
                });
                return currentPos;
            }

        }

    }
    public void WrongShelfPosCalculate(Book book)
    {
        currentPos.x -= (addedBooks[addedBooks.Count - 1].thickness * sizeCoefficient + book.thickness * sizeCoefficient) / 2;


    }


}
