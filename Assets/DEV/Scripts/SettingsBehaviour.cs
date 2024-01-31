using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    public float[] yValues;
    public Sprite[] vibrationSprites;

    public RectTransform vibration;
    public RectTransform mainMenu;

    public bool settingsOpen = false;

    public float animationTime = 0.3f;

    public bool vibrationEnabled = true;

    private void Start()
    {
        int vib = PlayerPrefs.GetInt("Vibration", 1);
        vibrationEnabled = vib == 1;
        Taptic.tapticOn = vibrationEnabled;
        ToggleVibration(false);
    }

    public void ToggleSettings()
    {
        settingsOpen = !settingsOpen;
        if (settingsOpen)
        {
            vibration.DOAnchorPosY(yValues[0], animationTime);
            mainMenu.DOAnchorPosY(yValues[1], animationTime);
        }
        else
        {
            vibration.DOAnchorPosY(0f, animationTime);
            mainMenu.DOAnchorPosY(0f, animationTime);
        }
    }

    public void ToggleVibration(bool toggle = true)
    {
        if (toggle) vibrationEnabled = !vibrationEnabled;
        if (vibrationEnabled)
        {
            PlayerPrefs.SetInt("Vibration", 1);
            vibration.GetComponent<Image>().sprite = vibrationSprites[0];
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 0);
            vibration.GetComponent<Image>().sprite = vibrationSprites[1];
        }
        Taptic.tapticOn = vibrationEnabled;

    }

    public void GoMainMenu()
    {

    }
}
