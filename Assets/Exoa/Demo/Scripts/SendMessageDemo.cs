using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendMessageDemo : MonoBehaviour
{
    public Image img;
    public List<Sprite> sprites;

    public void StepHello()
    {
        img.sprite = sprites[0];
    }

    public void StepPopup()
    {
        img.sprite = sprites[1];
    }

    public void StepSendMessage()
    {
        img.sprite = sprites[2];
    }

    public void StepCustomize()
    {
        img.sprite = sprites[3];
    }
}
