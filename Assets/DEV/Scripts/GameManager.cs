using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int health = 3;
    [SerializeField] private List<Book> bookObjects = new List<Book>();

    public CanvasGroup winPanel;
    public CanvasGroup losePanel;

    public enum GameState
    {
        Playing,
        Waiting
    }

    public GameState state = GameState.Playing;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        winPanel = GameObject.Find("WinPanel").GetComponent<CanvasGroup>();
        losePanel = GameObject.Find("LosePanel").GetComponent<CanvasGroup>();
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        Application.targetFrameRate = 60;
        winPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => GoNextLevel());
        losePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RestartLevel());
        losePanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => GoToMainMenu());
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
        if (counter == temp)
        {
            LevelCompleted();
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
        DOVirtual.DelayedCall(3f, () =>
        {
            winPanel.gameObject.SetActive(true);
            state = GameState.Waiting;
            winPanel.DOFade(1f, 0.6f);
        });
        
    }
    public void GameOver()
    {
        DOVirtual.DelayedCall(3f, () =>
        {
            losePanel.gameObject.SetActive(true);
            state = GameState.Waiting;
            losePanel.DOFade(1f, 0.6f);
        });
        
    }

    public void CorrectShelf()
    {
        Debug.Log("CorrectShelf!!!");
    }

    public void Vibrate()
    {
        Taptic.Medium();
    }

    public void GoNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }













}
