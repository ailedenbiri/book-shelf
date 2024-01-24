using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public int health;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        health = 3;
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
