using DG.Tweening;
using Exoa.TutorialEngine;
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

    private int hintCount = 4;
    private TextMeshProUGUI hintCountText;
    [HideInInspector] public int shelfc;
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
        Application.targetFrameRate = 60;
        state = GameState.Waiting;

        //Camera animations at beginning
        Transform mainCam = Camera.main.transform;
        Transform cameraStartPos = GameObject.Find("CameraFirstPos").transform;
        Vector3 cameraTargetPos = mainCam.position;
        Quaternion cameraTargetRot = mainCam.rotation;
        mainCam.position = cameraStartPos.position;
        mainCam.rotation = cameraStartPos.rotation;
        mainCam.DOMove(cameraTargetPos, 1.2f).SetEase(Ease.InSine);
        mainCam.DORotateQuaternion(cameraTargetRot, 1.2f).SetEase(Ease.InSine).OnComplete(() =>
        {
            //GetHint();
            state = GameState.Playing;
            if (SceneManager.GetActiveScene().name == "LEVEL - 1")
            {
                MTutorialController.instance.LoadTutorial("T-1", 0.3f, -1);
            }
        });

        //ui elements settings
        //main buttons
        healthText = GameObject.Find("Text_HeartCount").GetComponent<TextMeshProUGUI>();
        healthText.text = health.ToString();
        heartImage = GameObject.Find("HealthBar").transform;
        //game end panels
        winPanel = GameObject.Find("WinPanel").GetComponent<CanvasGroup>();
        losePanel = GameObject.Find("LosePanel").GetComponent<CanvasGroup>();
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        winPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => GoNextLevel());
        losePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RestartLevel());
        losePanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => GoToMainMenu());

        GameObject.Find("Button_Restart").GetComponent<Button>().onClick.AddListener(RestartLevel);
        //GameObject.Find("Button_GoNextForce").GetComponent<Button>().onClick.AddListener(GoNextLevelForced);
        GameObject.Find("Text_Level").GetComponent<TextMeshProUGUI>().text = "Level " + (PlayerPrefs.GetInt("Index") + 1).ToString();

        GetAllBooksInScene();

        //add go home listener to button
        GameObject.Find("Button_Home").GetComponent<Button>().onClick.AddListener(GoToMainMenu);

        if (SceneManager.GetActiveScene().name == "LEVEL - 1")
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                foreach (var item in GameObject.FindObjectsOfType<ShelfGrid>())
                {
                    item.GetComponent<BoxCollider>().enabled = false;
                }
            });
        }
    }

    public void GetHint()
    {
        if (hintCount > 0)
        {
            BookController.instance.DropBookIfSelected();
            Sequence seq = DOTween.Sequence();
            CanvasGroup c = GameObject.Find("ShelfInfoCanvas").transform.GetChild(0).GetComponent<CanvasGroup>();
            c.gameObject.SetActive(true);
            c.DOKill();
            seq.SetId(c);
            seq.Append(c.DOFade(1f, 0.6f));
            seq.AppendCallback(() => state = GameState.Waiting);

        }
    }

    public void CloseHint()
    {
        Sequence seq = DOTween.Sequence();
        CanvasGroup c = GameObject.Find("ShelfInfoCanvas").transform.GetChild(0).GetComponent<CanvasGroup>();
        c.DOKill();
        seq.SetId(c);
        seq.Append(c.DOFade(0f, 0.6f));
        seq.AppendCallback(() => c.gameObject.SetActive(false));
        seq.AppendCallback(() => state = GameState.Playing);
        hintCount--;
        hintCountText.text = hintCount.ToString();
        if (hintCount == 0)
        {
            hintCountText.transform.parent.parent.GetComponent<Button>().interactable = false;
        }
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
        SaveLastLevel();
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

    public void GoNextLevelForced()
    {
        savedIndex = PlayerPrefs.GetInt("Index", 0);
        PlayerPrefs.SetInt("Index", savedIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveLastLevel()
    {
        savedIndex = PlayerPrefs.GetInt("Index", 0);
        PlayerPrefs.SetInt("Index", savedIndex + 1);
        Debug.Log("SavedIndex: " + savedIndex);
        PlayerPrefs.Save();
        Debug.Log("Saved!!!");
    }

}
