using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public RectTransform levelsParent;
    private int level;
 
    IEnumerator Start()
    {
        Time.timeScale = 1f;
        yield return new WaitForEndOfFrame();
        
        level = PlayerPrefs.GetInt("Index", 0); 
        
        levelsParent.DOAnchorPosX(-412.5f * level, 0f);
        levelsParent.transform.GetChild(level + 1).GetComponent<Image>().color = Color.yellow;
        foreach (Transform item in levelsParent.transform)
        {
            if (item.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                item.GetComponentInChildren<TextMeshProUGUI>().text = item.GetSiblingIndex().ToString();
            }
        }
    }
    public void StartGame()
    {
        int savedIndex = PlayerPrefs.GetInt("Index"); 
        Debug.Log(savedIndex + " index");
        if (savedIndex != 0)
        {
            Debug.Log("Son level");
            SceneManager.LoadScene(savedIndex);
        }
        else
        {
            int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
            SceneManager.LoadScene(selectedLevel + 1);
        }
    }
}

