using Exoa.TutorialEngine;
using UnityEngine;
using UnityEngine.UI;

public class DemoSendMessage : MonoBehaviour
{
    public Button startTutorialBtn;

    private void OnDestroy()
    {
        TutorialEvents.OnTutorialComplete -= OnTutorialCompleted;
    }
    void Start()
    {
        startTutorialBtn.onClick.AddListener(OnClickStartTutorial);
    }

    void OnClickStartTutorial()
    {
        TutorialController.IsSkippable = false;
        TutorialController.AutoNext = true;
        TutorialLoader.instance.Load("1.4");

        startTutorialBtn.gameObject.SetActive(false);
    }


    private void OnTutorialCompleted()
    {
        TutorialEvents.OnTutorialComplete -= OnTutorialCompleted;
        startTutorialBtn.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            !TutorialController.IsTutorialActive &&
            !TutorialController.IsTutorialComplete &&
            TutorialLoader.instance.tutorialLoaded)
        {
            TutorialController.instance.ForceNext();
        }
    }
}
