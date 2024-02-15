using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int health = 3;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Transform heartImage;
    [SerializeField] private List<Book> bookObjects = new List<Book>();
    public int savedIndex;
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
        
        state = GameState.Waiting;
        Transform mainCam = Camera.main.transform;
        Transform cameraStartPos = GameObject.Find("CameraFirstPos").transform;
        Vector3 cameraTargetPos = mainCam.position;
        Quaternion cameraTargetRot = mainCam.rotation;
        mainCam.position = cameraStartPos.position;
        mainCam.rotation = cameraStartPos.rotation;
        mainCam.DOMove(cameraTargetPos, 1.2f).SetEase(Ease.InSine);
        mainCam.DORotateQuaternion(cameraTargetRot, 1.2f).SetEase(Ease.InSine).OnComplete(() =>
        {
            state = GameState.Playing;
            FadeShelfInfos();
        });
        GameObject.Find("Button_Help").GetComponent<Button>().onClick.AddListener(FadeShelfInfos);
        healthText = GameObject.Find("Text_HeartCount").GetComponent<TextMeshProUGUI>();
        healthText.text = health.ToString();
        heartImage = GameObject.Find("HealthBar").transform;

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

    public void FadeShelfInfos()
    {
        Sequence seq = DOTween.Sequence();
        CanvasGroup c = GameObject.Find("ShelfInfoCanvas").GetComponent<CanvasGroup>();
        c.DOKill();
        seq.SetId(c);
        seq.Append(c.DOFade(1f, 0.6f));
        seq.Append(c.DOFade(0f, 0.6f).SetDelay(2f));
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
            healthText.text = health.ToString();
            heartImage.DOPunchScale(heartImage.lossyScale * 0.3f, 0.4f);
        }
        else if (health == 1)
        {
            health -= 1;
            Vibrate();
            GameOver();
            healthText.text = health.ToString();
            heartImage.DOPunchScale(heartImage.lossyScale * 0.3f, 0.4f);
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
        state = GameState.Waiting;
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
        SaveLastLevel();
    }

    public void GoToMainMenu()
    {
        SaveLastLevel();
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveLastLevel()
    {
        savedIndex= SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Index", savedIndex);
        Debug.Log("SavedIndex: " + savedIndex);
        PlayerPrefs.Save();
        Debug.Log("Saved!!!");
    }

}
