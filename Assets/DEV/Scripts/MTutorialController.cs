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

    private GameObject t1Hand;

    private void Update()
    {
        if (t1Hand != null)
        {
            MeshRenderer r = GameObject.Find("OnboardingBook").GetComponent<MeshRenderer>();
            Vector3 pos = r.bounds.center + Vector3.forward * -0.5f + Vector3.right * 0.12f + Vector3.up * -0.1f;
            t1Hand.transform.position = pos;
        }
        
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
                    if (tutorialName == "T-1")
                    {
                        t1Hand = GameObject.Find("t1Hand");
                    }
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
