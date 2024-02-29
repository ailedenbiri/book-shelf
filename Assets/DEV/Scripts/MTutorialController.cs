using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTutorialController : MonoBehaviour
{
    public static MTutorialController instance;
    private void Awake()
    {
        instance = this;
    }

    public void LoadTutorial(string tutorialName, float openTime = 0.3f, float duration = 4f, float delay = 0f)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            foreach (Transform item in transform)
            {
                if (item.name == tutorialName)
                {
                    Transform t = item;
                    Debug.Log(t.name + "Opened");
                    t.gameObject.SetActive(true);
                    t.GetComponent<CanvasGroup>().DOFade(1f, openTime);
                    if (duration != -1)
                    {
                        DOVirtual.DelayedCall(duration, () =>
                        {
                            t.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(() =>
                            {
                                t.gameObject.SetActive(false);
                            });
                        });
                    }
                    return;
                }
            }
        });

    }

    public bool IsTutorialActive()
    {
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    public void CloseTutorial(string tutorialName)
    {
        foreach (Transform item in transform)
        {
            if (item.name == tutorialName)
            {
                Transform t = item;
                Debug.Log(t.name + "Closed");
                t.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(() =>
                {
                    t.gameObject.SetActive(false);
                });
                return;
            }
        }
    }
}
