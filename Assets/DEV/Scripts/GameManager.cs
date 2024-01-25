using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int health=3;
    [SerializeField] private List<GameObject> bookObjects = new List<GameObject>();
    
    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        allBooksOnScene();
        
    }

    public void reduceBooks()
    {
        bookObjects.RemoveAt(bookObjects.Count - 1);
        if(bookObjects.Count == 0 )
        {
            LevelCompleted();
        }
    }

    private void allBooksOnScene()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("book"))
            {
                bookObjects.Add(obj);
            }
        }
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
