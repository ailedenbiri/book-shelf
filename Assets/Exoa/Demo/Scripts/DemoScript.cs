using Exoa.TutorialEngine;
using UnityEngine;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour
{

    public Button playBtn;
    public Button action1Btn;
    public Button action2Btn;
    public Button startTutorialBtn;

    private void OnDestroy()
    {
        TutorialEvents.OnTutorialComplete -= OnTutorialCompleted;
    }
    void Start()
    {
        playBtn.onClick.AddListener(OnClickPlay);
        startTutorialBtn.onClick.AddListener(OnClickStartTutorial);
        playBtn.gameObject.SetActive(false);
        action1Btn.gameObject.SetActive(false);
        action2Btn.gameObject.SetActive(false);
    }

    void OnClickStartTutorial()
    {
        TutorialController.IsSkippable = false;
        TutorialLoader.instance.Load("1.1");

        startTutorialBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        action1Btn.gameObject.SetActive(true);
        action2Btn.gameObject.SetActive(true);
    }

    void OnClickPlay()
    {
        TutorialController.IsSkippable = true;
        TutorialLoader.instance.Load("1.2", true);
        TutorialEvents.OnTutorialComplete += OnTutorialCompleted;

        playBtn.gameObject.SetActive(false);


    }

    private void OnTutorialCompleted()
    {
        TutorialEvents.OnTutorialComplete -= OnTutorialCompleted;
        startTutorialBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(true);
    }
    public bool clickAnywhereToGONext = true;
    private void Update()
    {
        if (clickAnywhereToGONext &&
            TutorialController.IsTutorialActive &&
            Input.GetMouseButtonDown(0))
        {
            TutorialController.instance.Next();
        }
    }
}
