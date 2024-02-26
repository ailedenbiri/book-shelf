using Exoa.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleButton : MonoBehaviour
{
    public Springs scaleSpringSetting;
    private Vector2Spring scaleSping;

    private bool toggled;
    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        print("button OnClicked");
        toggled = !toggled;
    }

    // Update is called once per frame
    void Update()
    {
        (transform as RectTransform).localScale = scaleSpringSetting.UpdateSpring(ref scaleSping, Vector2.one * (toggled ? 1.4f : 1f));
    }
}
