using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int health = 3;
    [SerializeField] private List<Book> bookObjects = new List<Book>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GetAllBooksInScene();
    }

    public bool CountBooks()
    {
        int temp = bookObjects.Count;
        int counter = 0;
        foreach (var item in bookObjects)
        {
            if (item.placed)
            {
                counter++;
            }
        }
        return counter == temp;
    }

    private void GetAllBooksInScene()
    {
        bookObjects = new List<Book>(GameObject.FindObjectsOfType<Book>());
    }
    public void WrongShelf()
    {
        if (health > 1)
        {
            health -= 1;
            Vibrate();
        }
        else if (health == 1)
        {
            health -= 1;
            Vibrate();
            GameOver();
        }
        else
        {
            Debug.Log("Something is wrong!!!");
        }
    }

    public void LockBooks()
    {
        foreach (var item in bookObjects)
        {
            if (!item.placed)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public void UnlockBooks()
    {
        foreach (var item in bookObjects)
        {
            if (!item.placed)
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    public void LevelCompleted()
    {
        Debug.Log("Congratulations!!!");
    }
    public void GameOver()
    {
        Debug.Log("GameOver!!!");
    }

    public void CorrectShelf()
    {
        Debug.Log("CorrectShelf!!!");
    }

    public void Vibrate()
    {
        Handheld.Vibrate();
    }
    public void StartMenu()
    {

    }
    public void RestartMenu()
    {
        //RestartMenu.setActive(true);
    }

    public void PauseMenu()
    {

    }













}
