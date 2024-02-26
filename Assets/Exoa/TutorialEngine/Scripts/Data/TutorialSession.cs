using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Exoa.TutorialEngine
{
    [System.Serializable]
    public class TutorialSession
    {
        [System.Serializable]
        public struct TutorialStep
        {
            public string text;
            public string target_obj;
            public string sendMessage;

            public bool isClickable;
            public bool isReplacingNextButton;
        }
        public TutorialStep[] steps;
    }
}
